using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhilosopherStone : Boss {
    //Phase 1
    List<ProjectileModule> modules = new List<ProjectileModule>();
    Vector3 currentDir;
    private float speed = 5.0f;

    new private void Start()
    {
        base.Start();
        currentDir = new Vector2(1, 1);
        modules.Add(GetComponent<ProjectileModule>());
        StartCoroutine(FireCycle());
    }

    IEnumerator FireCycle()
    {
        while(health > 0)
        {
            modules[0].ShootHalo();
            yield return new WaitForSeconds(0.5f);
            modules[0].SpawnHalo();
            yield return new WaitForSeconds(0.5f);
        }
    }

    internal override void Move()
    {
        base.Move();
        transform.position += currentDir * Time.deltaTime * speed;
    }

    internal override void Attack()
    {
        base.Attack();
    }

    new private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Bounce(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Bounce(collision);
    }

    private void Bounce(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[4];
        collision.GetContacts(contacts);
        for (int i = 0; i < contacts.Length; i++)
        {
            if (contacts[i].point != Vector2.zero)
            {
                if (Mathf.Abs(contacts[i].point.x) > Mathf.Abs(contacts[i].point.y))
                {
                    if (contacts[i].point.x > 0 && currentDir.x > 0) currentDir = new Vector3(-1, currentDir.y);
                    if (contacts[i].point.x < 0 && currentDir.x < 0) currentDir = new Vector3(1, currentDir.y);
                }
                else
                {
                    if (contacts[i].point.y > 0 && currentDir.y > 0) currentDir = new Vector3(currentDir.x, -1);
                    if (contacts[i].point.y < 0 && currentDir.y < 0) currentDir = new Vector3(currentDir.x, 1);
                }
            }
        }
    }
}
