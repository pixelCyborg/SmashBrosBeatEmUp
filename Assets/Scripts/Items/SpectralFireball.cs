using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralFireball : Projectile {
	new private void Start()
	{
		base.Start();
		body = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		Vector2 v = body.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy") return;

        if (collision.transform.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage, Vector3.zero);
            }
            //Destroy(gameObject);
        }
    }
}

