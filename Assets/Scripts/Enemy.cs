using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float walkSpeed = 1.0f;
	public int health = 1;
    public int damage = 1;
	public bool moveDisabled { get; private set; }
	private SpriteRenderer sprite;
	private Color origColor;
    private Healthbar healthbar;

	private bool facingRight = true;
    //Vector2 origScale;
    private Rigidbody2D body;
    private Collider2D playerCol;
    private Collider2D bodyCol;
    private static GameObject coinPrefab;
    public ContactFilter2D groundFilter;
    public float lungeWindup = 0.8f;
    private bool grounded;
    private bool lungeReady = true;
    public Vector2 lungeForce;
    public float detectionRadius = 8.0f;

    public Transform target;
    private bool dead = false;

	// Use this for initialization
	void Start () {
		bodyCol = GetComponent<Collider2D> ();
        playerCol = FindObjectOfType<Player>().GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer> ();
		origColor = sprite.color;
        healthbar = GetComponentInChildren<Healthbar>();

        if (coinPrefab == null)
        {
            coinPrefab = Resources.Load("Coin") as GameObject;
        }
	}

    // Update is called once per frame
    void Update() {
        if (dead) return;

        grounded = false;
        Collider2D[] results = new Collider2D[3];
        if (Physics2D.OverlapBox((Vector2)transform.position - Vector2.up * 0.8f, new Vector2(0.6f, 0.2f), 0, groundFilter, results) > 0)
        {
            grounded = true;
        }

		if (moveDisabled) {
			return;
		}

        target = null;
        if(Vector3.Distance(playerCol.transform.position, transform.position) < detectionRadius)
        {
            target = playerCol.transform;
        }

        //If colliding with player, hit them
        if (bodyCol.IsTouching(playerCol)) {
            Player player = playerCol.GetComponent<Player>();
            Vector2 storedVel = body.velocity;
            player.TakeDamage(damage, new Vector2(storedVel.x * 2, 5));
            body.velocity = Vector2.zero;
            body.AddForce(new Vector2(-storedVel.x * 4, 5), ForceMode2D.Impulse);
        }

        //If a target is found, chase it
        if (target != null && grounded)
        {
            if (target.position.x - transform.position.x < 0 && facingRight)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                facingRight = !facingRight;
            }
            else if (target.position.x - transform.position.x > 0 && !facingRight)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                facingRight = !facingRight;
            }

            body.velocity = Vector2.right * walkSpeed * (facingRight ? 2.0f : -2.0f);

            if (Vector2.Distance(transform.position, target.position) < 8.0f)
            {
                Lunge();
            }
        }
        //Otherwise, continue normal behavior
        else
        {
            if (grounded)
            {
                body.velocity = Vector2.right * walkSpeed * (facingRight ? 1 : -1);

                Debug.DrawLine((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x - Vector2.up * 0.8f), (Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x) - Vector2.up * 1.1f);
                if (Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x),
                        new Vector2(0.1f, 0.5f), 0, groundFilter, results) > 0 ||
                    Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x) - Vector2.up * 0.8f,
                        new Vector2(0.1f, 0.1f), 0, groundFilter, results) == 0)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                    facingRight = !facingRight;
                }
            }
        }
	}

    private void Lunge()
    {
        if (!lungeReady) return;
        StartCoroutine(_Lunge());
    }

    private IEnumerator _Lunge()
    {
        lungeReady = false;
        moveDisabled = true;
        yield return new WaitForSeconds(lungeWindup);
        body.velocity = Vector2.zero;
        body.AddForce(new Vector2(lungeForce.x * transform.localScale.x, lungeForce.y), ForceMode2D.Impulse);
        int timeoutIndex = 0;
        while(grounded)
        {
            timeoutIndex++;
            if (timeoutIndex > 300) break;
            yield return new WaitForEndOfFrame();
        }
        while (!grounded)
        {
            timeoutIndex++;
            if (timeoutIndex > 300) break;
            yield return new WaitForEndOfFrame();
        }
        moveDisabled = false;
        yield return new WaitForSeconds(lungeWindup * 2);
        lungeReady = true;
    }

	public void TakeDamage(int damage, Vector2 enemyPos) {
		health -= damage;
        healthbar.SetLifeCount(health);
        body.velocity = Vector3.zero;
		body.AddForce (((Vector2)transform.position - (Vector2)enemyPos) * 5 * damage, ForceMode2D.Impulse);
		StartCoroutine (_TakeDamage (damage, enemyPos));
	}

    IEnumerator _TakeDamage(int damage, Vector2 enemyPos)
    {
        moveDisabled = true;
        sprite.color = (sprite.color == origColor ? Color.white : origColor);
        if (health < 1)
        {
            dead = true;
            moveDisabled = true;
            body.constraints = RigidbodyConstraints2D.None;
            DropCoins(enemyPos);
            GetComponent<SpriteRenderer>().color = Color.grey;
            //body.AddTorque(Random.Range(-180, 180));
            Vector3 oldPosition = Vector3.zero;
            Debug.Log(Vector3.Distance(transform.position, oldPosition));
            StopCoroutine("_Lunge");
            body.drag = 0.2f;
            while (Vector3.Distance(transform.position, oldPosition) > 0.02f)
            {
                oldPosition = transform.position;
                yield return new WaitForSeconds(0.1f);
            }
            Die(enemyPos);
        }
        else {
            yield return new WaitForSeconds(0.1f);
            moveDisabled = false;
        }
    }

    void Die(Vector2 enemyPos)
    {
        Destroy(GetComponent<Collider2D>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(this);
    }

    void DropCoins(Vector2 enemyPos)
    {
        int coins = Random.Range(5, 10);
        for (int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
            Rigidbody2D coinBody = coin.GetComponent<Rigidbody2D>();
            enemyPos.x += Random.Range(0, 2);
            enemyPos.y += Random.Range(-1, -3);
            coinBody.AddForce(((Vector2)transform.position - (Vector2)enemyPos).normalized * 5, ForceMode2D.Impulse);
        }
    }
}
