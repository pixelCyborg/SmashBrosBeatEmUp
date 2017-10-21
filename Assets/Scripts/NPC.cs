using System.Collections;
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

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();	
		sprite = GetComponent<SpriteRenderer> ();
		origColor = sprite.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (takingDamage) {
			sprite.color = (sprite.color == origColor ? Color.white : origColor);
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
		body.AddForce ((Vector2)transform.position - enemyPos, ForceMode2D.Impulse);
		StartCoroutine (_TakeDamage ());
	}

	IEnumerator _TakeDamage() {
		takingDamage = true;
		if (health < 1) {
			yield return new WaitForSeconds (1.0f);
			Destroy (gameObject);
		} else {
			yield return new WaitForSeconds (1.0f);
			takingDamage = false;
		}
	}
}
