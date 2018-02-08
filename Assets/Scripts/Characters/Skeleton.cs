using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy {
    //Lunge specific stuff
    private bool lungeReady = true;
    public float lungeWindup = 0.8f;
    public Vector2 lungeForce;
    Vector2 prevDistance;

    internal override void Attack(Transform target)
    {
        base.Attack(target);
        if (!lungeReady) return;
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
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                facingRight = !facingRight;
            }
        }
    }

    internal override void PursueTarget(Transform target)
    {
        base.PursueTarget(target);
        if (grounded)
        {
            if (lungeReady && Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x), new Vector2(0.1f, 0.5f), 0, groundFilter, results) > 0)
            {
                StartCoroutine(Jump());
            }

            if (target.position.x - transform.position.x < 0 && facingRight)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                facingRight = !facingRight;
            }
            else if (target.position.x - transform.position.x > 0 && !facingRight)
            {
                transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                facingRight = !facingRight;
            }
            
            if(Vector2.Distance(transform.position, target.position) > 4)
                body.velocity = Vector2.right * moveSpeed * (facingRight ? 1.5f : -1.5f);
        }
    }

    private IEnumerator Jump()
    {
        lungeReady = false;
        yield return new WaitForSeconds(lungeWindup);

        body.AddForce(new Vector2(lungeForce.x * transform.localScale.x * 0.5f, lungeForce.y * 3.0f), ForceMode2D.Impulse);
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
        yield return new WaitForSeconds(lungeWindup * 2);
        lungeReady = true;
    }

    private IEnumerator _Lunge()
    {
        lungeReady = false;
        //moveDisabled = true;
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
        //moveDisabled = false;
        yield return new WaitForSeconds(lungeWindup * 2);
        lungeReady = true;
    }
}
