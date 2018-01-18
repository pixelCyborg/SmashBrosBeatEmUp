using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowBolt : MonoBehaviour {
    public float boltSpeed = 1.0f;
    int damage = 1;
    Rigidbody2D body;
    Vector3 dir;
    float rotationSpeed = 10.0f;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Shoot(float x, float y)
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine(StartTimeout());
        body.velocity = new Vector2(x, y) * boltSpeed;
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
