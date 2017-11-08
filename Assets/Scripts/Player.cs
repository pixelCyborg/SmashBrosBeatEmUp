using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	CharacterController2D controller;
	Animator anim;

    private float gravity = 1;
    public float speed = 2.0f;
	public float jumpForce = 100.0f;
    private Vector2 moveDirection;
	public Collider2D groundCheck;
	public Collider2D attackCollider;

	//Stats
	private int health = 5;
	private int damage = 1;
	Vector2 origScale;
	private bool takingDamage = false;
    public bool isAttacking;
    public AudioClip punchSound;
    private AudioSource source;

    public Text coinCounter;

    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
		controller = GetComponent<CharacterController2D> ();
		anim = GetComponent<Animator> ();
		origScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if (Grounded() && Input.GetKeyDown (KeyCode.W)) {
			Jump ();
		}

        float x = Input.GetAxis("Horizontal");
        //Move(x);

        if (Input.GetKeyDown (KeyCode.Space)) {
			Attack ();
		}

		if (isAttacking && attackCollider != null) {
			CheckAttack ();
		}
	}

    bool Grounded()
    {
        Collider2D[] results = new Collider2D[3];
        return groundCheck.OverlapCollider(new ContactFilter2D(), results) > 0;
    }

    void Move(float x) {
        if (controller == null) return;
        //body.velocity = movement;
        moveDirection.x = -x * speed;

        moveDirection.y *= gravity * Time.deltaTime;

		if(x != 0)
			transform.localScale = x > 0 ? origScale : new Vector2 (-origScale.x, origScale.y);

        Debug.Log(moveDirection);
        controller.move(moveDirection);
	}

	void Jump() {
        Debug.Log("Jump!");
        moveDirection.y = -jumpForce;
	}

	void Attack() {
        if (punchSound != null) source.PlayOneShot(punchSound, 0.5f);
        anim.SetTrigger ("Attack");
	}

	void CheckAttack() {
		Collider2D[] results = new Collider2D[5];
		attackCollider.OverlapCollider (new ContactFilter2D (), results);
		for (int i = 0; i < results.Length; i++) {
			if (results [i] != null) {
				if (results [i].tag == "Enemy") {
					NPC enemy = results [i].GetComponent<NPC> ();
					if (!enemy.takingDamage) {
						enemy.TakeDamage (damage, transform.position);
					}
				}
			}
		}
	}

    public void PickUpCoin()
    {

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
