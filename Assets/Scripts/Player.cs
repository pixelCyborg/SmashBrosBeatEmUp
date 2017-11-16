﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    Inventory inventory;
	UnityStandardAssets._2D.PlatformerCharacter2D controller;
    Rigidbody2D body;
	Animator anim;

    private float gravity = 1;
    public float speed = 2.0f;
	public float jumpForce = 100.0f;
    private Vector2 moveDirection;
	public Collider2D groundCheck;

	//Stats
	private int health = 5;
	private int damage = 1;
	Vector2 origScale;
	private bool takingDamage = false;
    public bool isAttacking;
    public AudioClip punchSound;
    private AudioSource source;

    public Text coinCounter;
    public GameObject potionPrefab;
    public GameObject backpack;


    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
		controller = GetComponent<UnityStandardAssets._2D.PlatformerCharacter2D> ();
		anim = GetComponent<Animator> ();
        inventory = Inventory.instance;
		origScale = transform.localScale;
        potionPrefab = Resources.Load("Potion") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Grounded() && Input.GetKeyDown (KeyCode.W)) {
			Jump ();
		}

        if (Input.GetMouseButtonDown(0))
        {
            ThrowPotion();
        }

        float x = Input.GetAxis("Horizontal");
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
	}

	void Jump() {
        moveDirection.y = -jumpForce;
	}

    void ThrowPotion()
    {
        GameObject thrownPotion = Instantiate(potionPrefab, transform.position, potionPrefab.transform.rotation);
        thrownPotion.GetComponent<Potion>().Throw(body.velocity);
    }

    public void PickUpCoin(Coin coin)
    {

    }

    public void PickUp(Item item)
    {
        inventory.AddToInventory(item);
        item.transform.SetParent(backpack.transform);
        item.transform.localPosition = Vector2.zero;
        item.gameObject.SetActive(false);
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
