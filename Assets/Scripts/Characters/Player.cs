using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player instance;
    SpriteRenderer spriteRenderer;
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
    private int maxHealth;
	private int damage = 1;
	Vector2 origScale;
    private AudioSource source;

    public Text coinCounter;
    public GameObject potionPrefab;
    public GameObject backpack;
    private Hotbar hotbar;
    public bool takingDamage = false;
    public float recoverTime = 1.0f;
    private Crossbow crossbow;

    LightSource torch;

    public Healthbar healthbar;

    // Use this for initialization
    void Start () {
        instance = this;
        maxHealth = health;
        body = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
		controller = GetComponent<PlatformerCharacter2D> ();
		anim = GetComponent<Animator> ();
        inventory = Inventory.instance;
        hotbar = inventory.transform.parent.GetComponentInChildren<Hotbar>();
        crossbow = GetComponentInChildren<Crossbow>();
		origScale = transform.localScale;
        potionPrefab = Resources.Load("Potion") as GameObject;
        healthbar.SetLifeCount(health);
        spriteRenderer = GetComponent<SpriteRenderer>();

        torch = LightingManager.instance.CreateLightSource(transform);
	}

    private void Update()
    {
        if (takingDamage) return;

        if(Input.GetButtonDown("Fire1"))
        {
            crossbow.FireCrossbow();
        }

        if(Input.GetButtonDown("Fire2"))
        {
            ThrowPotion();
        }
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        health = maxHealth;
        healthbar.SetLifeCount(health);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        takingDamage = false;
        controller.enabled = true;
    }

    public void TakeDamage(int damage, Vector2 impact)
    {
        if (takingDamage) return;


        CameraShake.AddShake(damage * 0.25f);
        takingDamage = true;
        health -= damage;
        body.AddForce(impact * damage, ForceMode2D.Impulse);
        healthbar.SetLifeCount(health);
        if (health > 0) StartCoroutine(_TakeDamage());
        else StartCoroutine(_Die());

    }

    IEnumerator _TakeDamage()
    {
        yield return new WaitForSeconds(recoverTime);
        takingDamage = false;
    }

    IEnumerator _Die()
    {
        body.constraints = RigidbodyConstraints2D.None;
        controller.enabled = false;
        spriteRenderer.color = Color.grey;
        yield return new WaitForSeconds(3.0f);
        CanvasManager.ShowGameOver();
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
        thrownPotion.properties = new List<Property>(potionBlock.properties);
        thrownPotion.Throw(transform);
    }

    public void PickUp(Coin coin)
    {
        Inventory.AddCoin();
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
