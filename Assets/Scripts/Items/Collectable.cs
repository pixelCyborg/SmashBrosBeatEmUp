using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : Interactable {
    private static Transform target;
    public Item item = new Item();
    Rigidbody2D body;

    const int TIMEOUT = 10;
    const float VACUUM_RADIUS = 10.0f;
    const float moveSpeed = 10.0f;

    bool vacuumed = false;
    bool tangible = false;

    void Start()
    {
        StartCoroutine(TimeOut());
        body = GetComponent<Rigidbody2D>();
        if (gameObject.name.Contains("(Clone)")) gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);

        if (item.properties == null)
        {
            item.properties = new List<Property>();
        }
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        if (rend != null) item.sprite = rend.sprite;

        if (string.IsNullOrEmpty(item.itemName))
        {
            item.itemName = gameObject.name;
        }

        if (string.IsNullOrEmpty(item.description))
        {
            item.description = "";
            for (int i = 0; i < item.properties.Count; i++)
            {
                item.description += item.properties[i].type + " " + item.properties[i].power + "\n";
            }
        }

        if(item.quantity < 1)
        {
            item.quantity = 1;
        }

        description = item.itemName;

        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
    }

    IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(0.5f);
        tangible = true;
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(TIMEOUT);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (tangible)
        {
            Vector2 velocity = target.position - transform.position;
            body.velocity = velocity.normalized *
                (moveSpeed + (VACUUM_RADIUS -
                (Mathf.Clamp(Vector2.Distance(transform.position, target.position), 0, VACUUM_RADIUS - 1))
            ) * 0.5f);

            if (Vector2.Distance(target.position, transform.position) < 0.3f)
            {
                if (target != null)
                {
                    target.GetComponent<Player>().PickUp(this);
                    Destroy(gameObject);
                }
            }
        }
    }
}
