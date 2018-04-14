using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    public float[] phasePercentages;
    public HealthbarFill healthbar;

    private int phase;

    internal virtual void Move() { }
    internal virtual void Attack() { }

    internal void Start()
    {
        if (healthbar != null) healthbar.Initialize(health);
    }

    //Update healthbar and temp disable movement when taking damage
    public void TakeDamage(int damage, Transform origin = null)
    {
        if (health <= 0) return;
        health -= damage;

        CameraShake.AddShake(0.2f); 

        if (health < 1)
        {
            CameraShake.AddShake(0.2f);
            Die();
        }
        else
        {
            //StartCoroutine(DamageTimeout(true));
        }

        healthbar.SetHealth(health);
    }

    private void DamageFlash()
    {

    }

    IEnumerator _DamageFlash()
    {
        yield return new WaitForSeconds(0.1f);
    }

    private void Die()
    {
        Destroy(gameObject);
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
