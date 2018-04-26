using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
    private Transform _target;
    Rigidbody2D body;
    private bool tangible = false;
    public bool vacuumed = false;
    public AudioClip coinSound;
    const int COIN_TIMEOUT = 10;
    const float VACUUM_RADIUS = 10.0f;
    const float moveSpeed = 10.0f;

    private void Start()
    {
        StartCoroutine(CoinTimeout());
        body = GetComponent<Rigidbody2D>();
        _target = FindObjectOfType<Player>().transform;
    }

    IEnumerator CoinTimeout()
    {
        yield return new WaitForSeconds(0.5f);
        tangible = true;
        GetComponent<Collider2D>().isTrigger = true;
        yield return new WaitForSeconds(COIN_TIMEOUT);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (tangible)
        {
            Vector2 velocity = _target.position - transform.position;
            body.velocity = velocity.normalized *
                (moveSpeed + (VACUUM_RADIUS -
                (Mathf.Clamp(Vector2.Distance(transform.position, _target.position), 0, VACUUM_RADIUS - 1))
            ) * 0.5f);

            if (Vector2.Distance(_target.position, transform.position) < 0.3f)
            {
                if (_target != null)
                {
                    _target.GetComponent<Player>().PickUp(this);
                    Destroy(gameObject);
                }
            }
        }
    }
}
