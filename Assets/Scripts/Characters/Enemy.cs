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
    public float recoveryTime = 0.8f;
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
    internal Collider2D bodyCol;

    internal virtual void Move() { }
    internal virtual void OnDamage(Transform _target) { }
    internal virtual void PursueTarget(Transform _target) { }
    internal virtual void Attack(Transform _target) { }
    internal virtual void OnDie() { }

	// Use this for initialization
	internal void Start () {
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
    internal void Update() {
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
        player.TakeDamage(damage, new Vector2(storedVel.x, 10));
        body.velocity = Vector2.zero;
        body.AddForce(new Vector2(-storedVel.x * 1.5f, 10), ForceMode2D.Impulse);
        StartCoroutine(DamageTimeout(true));
        OnDamage(target);
    }

    IEnumerator DamageTimeout(bool takingDamage)
    {
        float currentTime = Time.time;
        moveDisabled = true;
        while (Time.time - currentTime < recoveryTime)
        {
            yield return new WaitForSeconds(0.1f);
            if (takingDamage && !dead) sprite.color = sprite.color == origColor ? Color.grey : origColor;
        }
        moveDisabled = false;
        if (takingDamage && !dead) sprite.color = origColor;
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
        Vector2 offset = Vector2.zero;

        if (bodyCol.GetType() == typeof(CapsuleCollider2D))
        {
            offset = Vector2.up * (((CapsuleCollider2D)bodyCol).size.y / 1.9f);
        }
        else if (bodyCol.GetType() == typeof(BoxCollider2D))
        {
            offset = Vector2.up * (((BoxCollider2D)bodyCol).size.y / 1.9f);
        }
        else if (bodyCol.GetType() == typeof(CircleCollider2D))
        {
            offset = Vector2.up * (((CircleCollider2D)bodyCol).radius / 1.9f);
        }
        else offset = Vector2.up;

        if (Physics2D.OverlapBox((Vector2)transform.position - offset, new Vector2(0.6f, 0.2f), 0, groundFilter, results) > 0)
        {
            grounded = true;
        }
    }

    //Update healthbar and temp disable movement when taking damage
	public void TakeDamage(int damage, Vector2 enemyPos, Transform origin = null) {
        if (health <= 0) return;
		health -= damage;
        healthbar.SetLifeCount(health);
        body.velocity = Vector3.zero;
		body.AddForce (((Vector2)transform.position - (Vector2)enemyPos) * 5 * damage, ForceMode2D.Impulse);
        if (origin != null)
        {
            target = origin;
        }

        CameraShake.AddShake(0.2f);
        moveDisabled = true;

        if (health < 1)
        {
            CameraShake.AddShake(0.2f);
            StartCoroutine(Die(enemyPos));
        }
        else
        {
            StartCoroutine(DamageTimeout(true));
        }
    }

    IEnumerator Die(Vector2 enemyPos)
    {
        StopCoroutine("DamageTimeout");
        dead = true;
        moveDisabled = true;
        GetComponentInChildren<MinimapIcon>().DisableIcon();
        sprite.color = origColor;
        body.constraints = RigidbodyConstraints2D.None;
        OnDie();
        DropCoins(enemyPos);
        Vector3 oldPosition = Vector3.zero;
        body.drag = 0.2f;

        yield return null;
        StatusEffect[] status = GetComponents<StatusEffect>();
        StopCoroutine("DamageTimeout");
        if (status != null)
        {
            for (int i = status.Length - 1; i >= 0; i--)
            {
                status[i].End();
            }
        }

        while (Vector3.Distance(transform.position, oldPosition) > 0.02f)
        {
            oldPosition = transform.position;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }

    void DropCoins(Vector2 enemyPos)
    {
        int coins = Random.Range(0, 5);
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
