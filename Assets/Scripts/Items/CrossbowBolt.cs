using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowBolt : Projectile {
    Transform origin;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player") return;

        if (collision.transform.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Boss boss = collision.gameObject.GetComponent<Boss>();

            if (enemy != null)
            {
                ApplyStatus(enemy);
                enemy.TakeDamage(damage, transform.position, origin);
            }

            if(boss != null)
            {
                boss.TakeDamage(damage, origin);
            }


            Destroy(gameObject);
        }
        else if (!CheckImpact(collision))
        {
            Destroy(gameObject);
        }
    }
}
