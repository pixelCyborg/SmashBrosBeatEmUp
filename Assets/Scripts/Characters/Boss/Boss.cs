using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    public float[] phasePercentages;

    private int phase;

    internal virtual void Move() { }
    internal virtual void Attack() { }

    //Update healthbar and temp disable movement when taking damage
    public void TakeDamage(int damage, Transform origin = null)
    {
        if (health <= 0) return;
        health -= damage;

        CameraShake.AddShake(0.2f); 

        if (health < 1)
        {
            CameraShake.AddShake(0.2f);
        }
        else
        {
            //StartCoroutine(DamageTimeout(true));
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    internal void AdvancePhase()
    {

    }

    internal void StartBossBattle()
    {

    }

    internal void BossBattleComplete()
    {

    }

    internal void BossBattleDefeat()
    {

    }
}
