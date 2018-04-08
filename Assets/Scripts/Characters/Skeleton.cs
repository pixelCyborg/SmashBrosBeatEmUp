using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy {
    public Sprite skull;
    //Lunge specific stuff
    private bool lungeReady = true;
    public float lungeWindup = 0.8f;
    public Vector2 lungeForce;
    Vector2 prevDistance;
    float xVel;

    ParticleSystem bones;

    new void Start()
    {
        base.Start();
        bones = GetComponentInChildren<ParticleSystem>();
        if(bones != null)
        bones.Stop();
    }

    internal override void Attack(Transform target)
    {
        base.Attack(target);
        if (!lungeReady) return;
        if (target.position.y > transform.position.y + 2) return;

        StartCoroutine(_Lunge());
    }

    internal override void Move()
    {
        base.Move();
        //MOVEMENT ABSTRACT HERE
        if (grounded)
        {
            Vector2 velocity = Vector2.right * moveSpeed * (facingRight ? 1 : -1);
            velocity.y = body.velocity.y;
            body.velocity = velocity;

            if (Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x),
                    new Vector2(0.1f, 0.5f), 0, groundFilter, results) > 0 ||
                Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x) - Vector2.up * 0.8f,
                    new Vector2(0.1f, 0.1f), 0, groundFilter, results) == 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
                facingRight = !facingRight;
            }
        }
    }

    internal override void PursueTarget(Transform target)
    {
        base.PursueTarget(target);
        if (grounded)
        {
            if (target.position.x - transform.position.x < -0.5f && facingRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
                facingRight = !facingRight;
                body.velocity = Vector2.right * moveSpeed * (facingRight ? 1.5f : -1.5f);
            }
            else if (target.position.x - transform.position.x > 0.5f && !facingRight)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
                facingRight = !facingRight;
                body.velocity = Vector2.right * moveSpeed * (facingRight ? 1.5f : -1.5f);
            }

            if (lungeReady && (Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 1.5f * transform.localScale.x), new Vector2(1f, 0.5f), 0, groundFilter, results) > 0
                || target.position.y > transform.position.y + 5))
            {
                StartCoroutine(Jump());
            }
        }
    }

    private IEnumerator Jump()
    {
        lungeReady = false;
        body.AddForce(new Vector2(0, lungeForce.y * 2f), ForceMode2D.Impulse);

        int timeoutIndex = 0;
        Vector3 vel = body.velocity;
        while (grounded)
        {
            timeoutIndex++;
            if (timeoutIndex > 300) break;
            yield return new WaitForEndOfFrame();
            vel = body.velocity;
            vel.x = moveSpeed * transform.localScale.x;
            body.velocity = vel;
        }
        while (!grounded)
        {
            timeoutIndex++;
            if (timeoutIndex > 300) break;
            yield return new WaitForEndOfFrame();
            vel = body.velocity;
            vel.x = moveSpeed * transform.localScale.x;
            body.velocity = vel;
        }
        yield return new WaitForSeconds(lungeWindup);
        lungeReady = true;
    }

    private IEnumerator _Lunge()
    {
        lungeReady = false;
        yield return new WaitForSeconds(lungeWindup);
        body.AddForce(new Vector2(lungeForce.x * transform.localScale.x, lungeForce.y), ForceMode2D.Impulse);
        int timeoutIndex = 0;
        while (grounded)
        {
            timeoutIndex++;
            if (timeoutIndex > 300) break;
            yield return new WaitForEndOfFrame();
        }
        while (!grounded)
        {
            timeoutIndex++;
            if (timeoutIndex > 300) break;
            yield return new WaitForEndOfFrame();
        }
        lungeReady = true;
    }

    internal override void OnDie()
    {
        base.OnDie();
        StopCoroutine("_Lunge");

        if (bones != null)
        {
            bones.Emit(5);
        }

        if (skull != null)
        {
            GetComponent<SpriteRenderer>().sprite = skull;
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            col.sharedMaterial = new PhysicsMaterial2D();
            col.sharedMaterial.bounciness = 0.5f;
            col.size = new Vector2(col.size.x, col.size.x);
            body.AddTorque(Random.Range(-180, 180));
            body.AddForce(new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)));
        }
    }
}
