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

    void ExplosionDamage()
    {
		Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, potion.blastRadius, 1 << LayerMask.NameToLayer("Enemy"));
        for (int i = 0; i < collisions.Length; i++)
        {
            NPC enemy = collisions[i].GetComponent<NPC>();
            if (enemy != null)
            {
				potion.ApplyStatus (enemy);
                enemy.TakeDamage((int)potion.damage, transform.position);
                //enemy.GetComponent<Rigidbody2D>().AddForce();
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
        ExplosionDamage();
        GetComponent<ParticleSystem>().Play();
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        body.isKinematic = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
