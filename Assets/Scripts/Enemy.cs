using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	public float walkSpeed = 1.0f;
	public int health = 1;
	public bool moveDisabled { get; private set; }
	private SpriteRenderer sprite;
	private Color origColor;
    private Healthbar healthbar;

	Rigidbody2D body;
	private bool facingRight = true;
	//Vector2 origScale;
	public Collider2D floorCheck;
    public Collider2D forwardCheck;
    public Collider2D groundCheck;
    private static GameObject coinPrefab;
    public ContactFilter2D groundFilter;
    public float lungeWindup = 0.8f;
    private bool grounded;
    public Vector2 lungeForce;

    public Transform target;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();	
		sprite = GetComponent<SpriteRenderer> ();
		origColor = sprite.color;
        healthbar = GetComponentInChildren<Healthbar>();

        if (coinPrefab == null)
        {
            coinPrefab = Resources.Load("Coin") as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
        grounded = false;
        Collider2D[] results = new Collider2D[3];
        if (groundCheck.OverlapCollider(groundFilter, results) > 0) grounded = true;

		if (moveDisabled) {
			return;
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

                if (floorCheck.OverlapCollider(groundFilter, results) == 0 || forwardCheck.OverlapCollider(groundFilter, results) > 0)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                    facingRight = !facingRight;
                }
            }
        }
	}

    private void Lunge()
    {
        StartCoroutine(_Lunge());
    }

    private IEnumerator _Lunge()
    {
        moveDisabled = true;
        yield return new WaitForSeconds(lungeWindup);
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
    }

	public void TakeDamage(int damage, Vector2 enemyPos) {
		health -= damage;
        healthbar.SetLifeCount(health);
		body.AddForce (((Vector2)transform.position - (Vector2)enemyPos) * 15 * damage, ForceMode2D.Impulse);
		StartCoroutine (_TakeDamage (damage, enemyPos));
	}

    IEnumerator _TakeDamage(int damage, Vector2 enemyPos)
    {
        moveDisabled = true;
        sprite.color = (sprite.color == origColor ? Color.white : origColor);
        if (health < 1)
        {
            body.constraints = RigidbodyConstraints2D.None;
            //body.AddTorque(Random.Range(-180, 180));
            yield return new WaitForSeconds(1.0f);
            Die(enemyPos);
        }
        else {
            yield return new WaitForSeconds(1.0f);
            moveDisabled = false;
            sprite.color = (sprite.color == origColor ? Color.white : origColor);
        }
    }

    void Die(Vector2 enemyPos)
    {
        DropCoins(enemyPos);
        GetComponent<SpriteRenderer>().color = Color.grey;
        GetComponentInChildren<ParticleSystem>().Play();
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
