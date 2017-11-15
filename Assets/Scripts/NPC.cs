﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour {
	public float walkSpeed = 1.0f;
	public int health = 1;
	public bool takingDamage { get; private set; }
	private SpriteRenderer sprite;
	private Color origColor;

	Rigidbody2D body;
	private bool facingRight = true;
	//Vector2 origScale;
	public Collider2D floorCheck;
    private static GameObject coinPrefab;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();	
		sprite = GetComponent<SpriteRenderer> ();
		origColor = sprite.color;
        if (coinPrefab == null)
        {
            coinPrefab = Resources.Load("Coin") as GameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (takingDamage) {
			return;
		}

		body.AddRelativeForce(Vector2.right * walkSpeed * (facingRight ? 1 : -1), ForceMode2D.Impulse);

		Collider2D[] results = new Collider2D[3];
		if (floorCheck.OverlapCollider (new ContactFilter2D (), results) == 0) {
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
			facingRight = !facingRight;
		}
	}

	public void TakeDamage(int damage, Vector2 enemyPos) {
		health -= damage;
		body.AddForce (((Vector2)transform.position - (Vector2)enemyPos) * 15 * damage, ForceMode2D.Impulse);
		StartCoroutine (_TakeDamage (damage, enemyPos));
	}

    IEnumerator _TakeDamage(int damage, Vector2 enemyPos)
    {
        takingDamage = true;
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
            takingDamage = false;
            sprite.color = (sprite.color == origColor ? Color.white : origColor);
        }
    }

    void Die(Vector2 enemyPos)
    {
        int coins = Random.Range(1, 4);
        for(int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
            Rigidbody2D coinBody = coin.GetComponent<Rigidbody2D>();
            enemyPos.x += Random.Range(0, 2);
            enemyPos.y += Random.Range(-1, -3);
            coinBody.AddForce(((Vector2)transform.position - (Vector2)enemyPos), ForceMode2D.Impulse);
            GetComponent<SpriteRenderer>().color = Color.grey;
            GetComponentInChildren<ParticleSystem>().Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(this);
        }
    }
}