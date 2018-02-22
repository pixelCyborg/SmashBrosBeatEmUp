using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFireball : Projectile {
	new private void Start()
	{
		base.Start();
		body = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector2 v = body.velocity;
		float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg - 90.0f;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public void Shoot(float x, float y, int _damage = 1)
	{
		damage = _damage;
		body = GetComponent<Rigidbody2D>();
		StartCoroutine(StartTimeout());
		body.AddForce(new Vector2(x, y).normalized * Crossbow.instance.boltSpeed, ForceMode2D.Impulse);
	}

	IEnumerator StartTimeout()
	{
		yield return new WaitForSeconds(5.0f);
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.transform.tag == "Enemy") return;

		if (collision.transform.tag == "Player")
		{
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();
			if(enemy != null)
			{
				enemy.TakeDamage(damage, transform.position);
			}
			Destroy(gameObject);
		}
		else if (!CheckImpact(collision))
		{
			Destroy(gameObject);
		}
	}
}

