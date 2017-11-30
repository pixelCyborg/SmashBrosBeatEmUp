using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {
	public enum Properties {
		Bouncy,
		//Makes Potions ricochet
		//Can make surfaces bouncy
		Impact, 
		//Will apply extra force to characters hit by the blast
		//Can be used for a double jump
		Puddle
		//Creates a surface of whatever element the potion is
	}

	public enum Elements {
		//Damage over time
		Fire,
		//Freezes/Slows Enemis
		//Slippery surfaces(?)
		Ice, 
		//Heals living, damages undead
		Holy
	}

    Rigidbody2D body;
    public float throwForce = 1.0f;
    public float blastRadius = 1.0f;
    public float damage = 1.0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Throw(Vector2 playerVel)
    {
        if (body == null) body = GetComponent<Rigidbody2D>();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        body.AddTorque(Random.Range(-50, 50));
        body.AddForce(((mouseWorldPos - (Vector2)transform.position).normalized) * throwForce, ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Explode();
    }

    void Explode()
    {
        StartCoroutine(_Explode());
    }

    void ExplosionDamage()
    {
        Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, blastRadius, 1 << LayerMask.NameToLayer("Enemy"));
        for(int i = 0; i < collisions.Length; i++)
        {
            NPC enemy = collisions[i].GetComponent<NPC>();
            if(enemy != null)
            {
                enemy.TakeDamage((int)damage, transform.position);
                //enemy.GetComponent<Rigidbody2D>().AddForce();
            }
        }
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
