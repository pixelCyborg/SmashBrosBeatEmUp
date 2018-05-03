using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static bool dead;
    public static Player instance;
    SpriteRenderer spriteRenderer;
    Inventory inventory;
	PlatformerCharacter2D controller;
    public Rigidbody2D body;
	Animator anim;

    private float gravity = 1;
    public float speed = 2.0f;
	public float jumpForce = 100.0f;
    private Vector2 moveDirection;
	public Collider2D groundCheck;

	//Stats
	public int health = 5;
    private int maxHealth;
	private int damage = 1;
	Vector2 origScale;

    public Text coinCounter;
    public GameObject potionPrefab;
    public GameObject backpack;
    private Hotbar hotbar;
    public bool takingDamage = false;
    public float recoverTime = 1.0f;
    private Crossbow crossbow;

    LightSource torch;
    public Healthbar healthbar;
    public CharacterAudioHandler audioHandler;

    private void Awake()
    {
        dead = false;
        instance = this;
    }

    // Use this for initialization
    void Start () {
        maxHealth = health;
        body = GetComponent<Rigidbody2D>();
        audioHandler.Initialize(GetComponent<AudioSource>());
		controller = GetComponent<PlatformerCharacter2D> ();
		anim = GetComponent<Animator> ();
        inventory = Inventory.instance;
        crossbow = GetComponentInChildren<Crossbow>();
		origScale = transform.localScale;
        potionPrefab = Resources.Load("Potion") as GameObject;
        healthbar.SetLifeCount(health);
        spriteRenderer = GetComponent<SpriteRenderer>();

        torch = LightingManager.instance.CreateLightSource(transform);
        SpawnPoint point = FindObjectOfType<SpawnPoint>();
        if(point != null)
        {
            transform.position = point.transform.position;
        }

	}

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            crossbow.FireCrossbow();
        }
    }

    public void Reset()
    {
        dead = false;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        health = maxHealth;
        healthbar.SetLifeCount(health);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        takingDamage = false;
        controller.enabled = true;
    }

    public void AddHealth(int amount)
    {
        health += amount;
        healthbar.SetLifeCount(health);
    }

    public void TakeDamage(int damage, Vector2 impact)
    {
        if (takingDamage) return;

        CameraShake.AddShake(damage * 0.25f);
        takingDamage = true;
        health -= damage;
        healthbar.SetLifeCount(health);
        body.AddForce(impact * damage, ForceMode2D.Impulse);

        if (health > 0) StartCoroutine(_TakeDamage());
        else StartCoroutine(_Die());

    }

    IEnumerator _TakeDamage()
    {
        audioHandler.PlayDamage();
        yield return new WaitForSeconds(recoverTime);
        takingDamage = false;
    }

    IEnumerator _Die()
    {
        dead = true;
        audioHandler.PlayDeath();
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
        audioHandler.PlayPickup();
        Inventory.AddCoin();
    }

    public void PickUp(Collectable collectable)
    {
        audioHandler.PlayPickup();
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
