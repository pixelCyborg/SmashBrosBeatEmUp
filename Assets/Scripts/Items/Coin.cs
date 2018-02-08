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
    const float moveSpeed = 3.0f;

    private void Start()
    {
        StartCoroutine(CoinTimeout());
        body = GetComponent<Rigidbody2D>();
        _target = FindObjectOfType<Player>().transform;
    }

    IEnumerator CoinTimeout()
    {
        yield return new WaitForSeconds(2.0f);
        tangible = true;
        yield return new WaitForSeconds(COIN_TIMEOUT);
        Destroy(gameObject);
    }

    private void Update()
    {
        if(tangible && !vacuumed && Vector3.Distance(_target.position, transform.position) < VACUUM_RADIUS)
        {
            vacuumed = true;
        }

        if (vacuumed)
        {
            Vector2 velocity = _target.position - transform.position;
            body.velocity = velocity.normalized *
                (moveSpeed + (VACUUM_RADIUS -
                (Mathf.Clamp(Vector2.Distance(transform.position, _target.position), 0, VACUUM_RADIUS - 1))
            ) * 0.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player player = other.gameObject.GetComponentInParent<Player>();
            if(player != null)
            {
                player.PickUp(this);
                Destroy(gameObject);
            }
        }
    }
}
