using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    Rigidbody2D body;
    public Potion potion;

    //Use for properties
    private int bounces;

    void Start()
    {
        //potion = new Potion();
        body = GetComponent<Rigidbody2D>();
        bounces = 0;
    }

    void OnCollisionEnter2D(Collision2D col)
	{
        if (CheckForProperty(Property.Type.Impact) > 0)
        {
            EffectDestructibles(col);
        }

        if(CheckForProperty(Property.Type.Bouncy) > 0)
        {
            AddBounceTiles(col);
        }

		if (col.gameObject.layer == 1 << LayerMask.NameToLayer ("Enemy")) {
			Explode ();
		}

		if (CheckForProperty (Property.Type.Bouncy) > bounces) {
			Bounce (col);
		} else {
			Explode ();
		}
	}

    public void Throw(Vector2 playerVel)
	{
		if (potion == null)
			potion = new Potion ();
		if (body == null)
			body = GetComponent<Rigidbody2D> ();
		Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		body.AddTorque (Random.Range (-50, 50));
		body.AddForce (((mouseWorldPos - (Vector2)transform.position).normalized) * potion.throwForce, ForceMode2D.Impulse);
	}


    void Explode()
    {
        StartCoroutine(_Explode());
    }

    void ExplosionDamage(Collider2D[] collisions)
    {
        for (int i = 0; i < collisions.Length; i++)
        {
            Enemy enemy = collisions[i].GetComponent<Enemy>();
            if (enemy != null)
            {
				potion.ApplyStatus (enemy);
                enemy.TakeDamage((int)potion.damage, transform.position);
                //enemy.GetComponent<Rigidbody2D>().AddForce();
            }
        }
    }

    Vector3[] GetTileWorldPositions(Collision2D collision)
    {
        Vector3[] collisionPoints = new Vector3[collision.contacts.Length];
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            ContactPoint2D hit = collision.contacts[i];
            Vector3 hitPosition = Vector3.zero;
            hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
            hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
            collisionPoints[i] = hitPosition;
        }
        return collisionPoints;
    }

    void AddBounceTiles(Collision2D collision)
    {
        Vector3[] collisions = GetTileWorldPositions(collision);
        for(int i = 0; i < collisions.Length; i++)
        {
            Bouncy.instance.SetBouncy(collisions[i]);
        }
    }

    void EffectDestructibles(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Destructibles>())
        {
            Destructibles destructibles = collision.gameObject.GetComponent<Destructibles>();
            Vector3[] collisions = GetTileWorldPositions(collision);
            for (int i = 0; i < collisions.Length; i++)
            {
                destructibles.DestroyTile(collisions[i]);
            }
        }
    }

    private int CheckForProperty(Property.Type type)
    {
        for (int i = 0; i < potion.properties.Length; i++)
        {
            if (potion.properties[i].type == type)
            {
                return potion.properties[i].power;
            }
        }

        return -1;
    }

    void Bounce(Collision2D col)
    {
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + col.relativeVelocity);
        body.AddForce(new Vector2(-col.relativeVelocity.x * 0.5f, col.relativeVelocity.y) * 0.8f , ForceMode2D.Impulse);
        bounces++;
    }

    IEnumerator _Explode()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, potion.blastRadius);
        ExplosionDamage(collisions);
        GetComponent<ParticleSystem>().Play();
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        body.isKinematic = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
