using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    internal Rigidbody2D body;
    public List<Property> properties;

    //Use for properties
    internal int damage = 1;
    private int bounces;
    private bool grenade = false;

    public void Start()
    {
        body = GetComponent<Rigidbody2D>();
        bounces = 0;
    }

    public bool CheckImpact(Collision2D col)
	{
		if (CheckForProperty (Property.Type.Bouncy) > bounces) {
			Bounce (col);
            return true;
		}

        return false;
    }

    public void CheckExplosion(Collision2D col)
    {
        if (col.gameObject.layer == 1 << LayerMask.NameToLayer("Enemy"))
        {
            Explode();
        }
        else
        {
            Explode();
        }

        if (CheckForProperty(Property.Type.Impact) > 0)
        {
            EffectDestructibles(col);
        }

        if (CheckForProperty(Property.Type.Bouncy) > 0)
        {
            AddBounceTiles(col);
        }
    }

    public void ApplyStatus(Enemy npc)
    {
        for (int i = 0; i < properties.Count; i++)
        {
            switch (properties[i].type)
            {
                case Property.Type.Fire:
                    npc.gameObject.AddComponent<Burning>();
                    break;

                case Property.Type.Ice:
                    npc.gameObject.AddComponent<Frozen>();
                    break;
            }
        }
    }

    public void Throw(Transform origin, float throwForce = 15.0f)
	{
		if (body == null)
			body = GetComponent<Rigidbody2D> ();

		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
        mouseWorldPos.z = origin.position.z;
		body.AddTorque (Random.Range (-50, 50));
		body.AddForce (((Vector3)mouseWorldPos - transform.position).normalized * throwForce, ForceMode2D.Impulse);
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
				ApplyStatus (enemy);
                enemy.TakeDamage(damage, transform.position);
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
        if (Bouncy.instance == null) return;

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
        for (int i = 0; i < properties.Count; i++)
        {
            if (properties[i].type == type)
            {
                return properties[i].power;
            }
        }

        return -1;
    }

    void Bounce(Collision2D col)
    {
        //Let the physics material do the work
        //body.velocity = -2 * (Vector2.Dot(body.velocity, col.contacts[0].normal) * col.contacts[0].normal + body.velocity);
        bounces++;
    }

    IEnumerator _Explode()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, 8.0f);
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
