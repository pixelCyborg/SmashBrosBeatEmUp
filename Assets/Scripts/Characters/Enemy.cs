using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    //User-set enemy properties 
    public float moveSpeed = 1.0f;
	public int health = 1;
    public int damage = 1;
    public float attackRange = 5.0f;
    public float detectionRadius = 8.0f;
    public ContactFilter2D groundFilter;

    //Shared general properties && components
    public bool moveDisabled { get; internal set; }
    internal static GameObject coinPrefab;
    internal bool facingRight = true;
    internal Rigidbody2D body;
    internal bool grounded;
    internal Collider2D[] results = new Collider2D[3];

    //Internal values
    private Transform target;
    private bool dead = false;
    private SpriteRenderer sprite;
    private Color origColor;
    private Healthbar healthbar;
    private Collider2D playerCol;
    private Collider2D bodyCol;

    internal virtual void Move() { }
    internal virtual void OnDamage(Transform _target) { }
    internal virtual void PursueTarget(Transform _target) { }
    internal virtual void Attack(Transform _target) { }

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
        CheckGround();

        //If unable to move, return
        if (dead || moveDisabled) return;
        CheckForPlayer();

        //If colliding with player, hit them
        if (bodyCol.IsTouching(playerCol)) {
            DamagePlayer();
        }

        //If a target is found, chase it

        //OnPursueTarget
        if (target != null)
        {
            PursueTarget(target);

            if (Vector2.Distance(transform.position, target.position) < attackRange)
            {
                Attack(target);
            }
        }
        //Otherwise, continue normal behavior
        else
        {
            Move();
        }
	}

    //This is specifically to damage the player on collision
    void DamagePlayer()
    {
        Player player = playerCol.GetComponent<Player>();
        Vector2 storedVel = body.velocity;
        player.TakeDamage(damage, new Vector2(storedVel.x, 5));
        body.velocity = Vector2.zero;
        body.AddForce(new Vector2(-storedVel.x, 5), ForceMode2D.Impulse);
        OnDamage(target);
    }

    //Check if player is within detection radius
    void CheckForPlayer()
    {
        target = null;
        if (Vector3.Distance(playerCol.transform.position, transform.position) < detectionRadius)
        {
            target = playerCol.transform;
        }
    } 

    //Basic Grounded check
    void CheckGround()
    {
        grounded = false;
        if (Physics2D.OverlapBox((Vector2)transform.position - Vector2.up * 0.8f, new Vector2(0.6f, 0.2f), 0, groundFilter, results) > 0)
        {
            grounded = true;
        }
    }

    //Update healthbar and temp disable movement when taking damage
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
