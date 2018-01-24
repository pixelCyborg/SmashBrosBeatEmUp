using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    Inventory inventory;
	PlatformerCharacter2D controller;
    Rigidbody2D body;
	Animator anim;

    private float gravity = 1;
    public float speed = 2.0f;
	public float jumpForce = 100.0f;
    private Vector2 moveDirection;
	public Collider2D groundCheck;

	//Stats
	private static int health = 5;
	public static int Health {
		get { return health; }
		set { health = value; }
	}
	private int damage = 1;
	Vector2 origScale;
    private AudioSource source;

    public Text coinCounter;
    public GameObject potionPrefab;
    public GameObject backpack;
    private Hotbar hotbar;
    public bool takingDamage = false;
    public float recoverTime = 1.0f;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
		controller = GetComponent<PlatformerCharacter2D> ();
		anim = GetComponent<Animator> ();
        inventory = Inventory.instance;
        hotbar = inventory.transform.parent.GetComponentInChildren<Hotbar>();
		origScale = transform.localScale;
        potionPrefab = Resources.Load("Potion") as GameObject;
	}

    public void TakeDamage(int damage, Vector2 impact)
    {
        if (takingDamage) return;

        takingDamage = true;
        health -= damage;
        body.AddForce(impact * damage, ForceMode2D.Impulse);
        StartCoroutine(_TakeDamage());
    }

    IEnumerator _TakeDamage()
    {
        yield return new WaitForSeconds(recoverTime);
        takingDamage = false;
    }

    bool Grounded()
    {
        Collider2D[] results = new Collider2D[3];
        return groundCheck.OverlapCollider(new ContactFilter2D(), results) > 0;
    }

    void ThrowPotion()
    {
        ItemTile tile = hotbar.CurrentTile();
        Potion potionBlock = null;
        if (tile.item != null && tile.item.potion != null)
        {
            potionBlock = tile.item.potion;
        }
        else
        {
            return;
        }

        Projectile thrownPotion = Instantiate(potionPrefab, transform.position, potionPrefab.transform.rotation).GetComponent<Projectile>();
        thrownPotion.GetComponent<SpriteRenderer>().sprite = tile.item.sprite;
        thrownPotion.potion = potionBlock;
        thrownPotion.Throw(transform);
    }

    public void PickUp(Coin coin)
    {

    }

    public void PickUp(Collectable collectable)
    {
        PickUp(collectable.item);
        collectable.transform.SetParent(backpack.transform);
        collectable.transform.localPosition = Vector2.zero;
        collectable.gameObject.SetActive(false);
    }

    public void PickUp(Item item)
    {
        inventory.AddToInventory(item);
    }
}
