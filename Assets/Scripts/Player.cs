using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	Rigidbody2D body;
	Animator anim;

	public float speed = 2.0f;
	public float jumpForce = 100.0f;
	public Collider2D groundCheck;
	public Collider2D attackCollider;

	//Stats
	private int health = 5;
	private int damage = 1;
	Vector2 origScale;
	private bool takingDamage = false;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		origScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		float x = Input.GetAxis ("Horizontal");
		Move (x);

		if (Grounded() && Input.GetKeyDown (KeyCode.W)) {
			Jump ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Attack ();
		}

		if (attackCollider != null) {
			CheckAttack ();
		}
	}

	bool Grounded() {
		Collider2D[] results = new Collider2D[3];
		return groundCheck.OverlapCollider(new ContactFilter2D(), results) > 0;
	}

	void Move(float x) {
		body.AddForce(new Vector2(x * speed, 0), ForceMode2D.Impulse);
		if(x != 0)
			transform.localScale = x > 0 ? origScale : new Vector2 (-origScale.x, origScale.y);
	}

	void Jump() {
		body.AddForce (new Vector2(0, jumpForce), ForceMode2D.Impulse);
	}

	void Attack() {
		anim.SetTrigger ("Attack");
	}

	void CheckAttack() {
		Collider2D[] results = new Collider2D[5];
		attackCollider.OverlapCollider (new ContactFilter2D (), results);
		for (int i = 0; i < results.Length; i++) {
			if (results [i] != null) {
				if (results [i].tag == "Enemy") {
					Debug.Log (results [i].gameObject.name);
					NPC enemy = results [i].GetComponent<NPC> ();
					if (!enemy.takingDamage) {
						enemy.TakeDamage (damage, transform.position);
					}
				}
			}
		}
	}

	public void TakeDamage (int amount) {
		health -= amount;
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
