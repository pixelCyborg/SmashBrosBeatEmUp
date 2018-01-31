using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowBolt : MonoBehaviour {
    int damage = 1;
    Rigidbody2D body;
    Vector3 dir;
    float rotationSpeed = 10.0f;

    private void Start()
    {
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
        body.velocity = new Vector2(x, y);
    }

    IEnumerator StartTimeout()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player") return;
        if (collision.transform.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.TakeDamage(damage, transform.position);
            }
        }
        //body.gravityScale = 1;
        Destroy(gameObject);
    }
}
