using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : Enemy {
    new private void Update()
    {
        base.Update();
        Vector2 v = body.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + (facingRight ? 0 : 180), Vector3.forward);
    }

    internal override void Move()
    {
        base.Move();

        Vector2 velocity = Vector2.right * moveSpeed * (facingRight ? 1 : -1);
        velocity.y = body.velocity.y;
        body.velocity = velocity;

        if (Physics2D.OverlapBox((Vector2)transform.position + (Vector2.right * 0.5f * transform.localScale.x),
                new Vector2(0.1f, 0.5f), 0, groundFilter, results) > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            facingRight = !facingRight;
        }
    }

    internal override void OnDamage(Transform _target)
    {
        base.OnDamage(_target);
        body.velocity = -body.velocity * 1.5f;
    }

    internal override void OnDie()
    {
        base.OnDie();
        body.gravityScale = 4.0f;
    }

    internal override void PursueTarget(Transform _target)
    {
        base.PursueTarget(_target);
        base.Attack(_target);
        Vector2 velocity = _target.position - transform.position;
        body.velocity = velocity.normalized * 
            (moveSpeed + (detectionRadius - 
            (Mathf.Clamp(Vector2.Distance(transform.position, _target.position), 0, detectionRadius - 1))
        ) * 0.5f);
    }
}
