using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health;
    public int damage = 1;

    private SpriteRenderer sprite;
    private Color origColor;
    private bool invuln = false;

    private float recoveryTime = 0.5f;
    public float[] phasePercentages;
    public HealthbarFill healthbar;
    private GameObject campfire;

    private int phase;

    internal virtual void Move() { }
    internal virtual void Attack() { }
    internal virtual void OnDie() { }

    internal void Start()
    {
        if (healthbar != null) healthbar.Initialize(health);
        sprite = GetComponent<SpriteRenderer>();
        origColor = sprite.color;
        campfire = FindObjectOfType<Campfire>().gameObject;
        campfire.SetActive(false);

    }

    internal void Update()
    {
        Move();
    }

    //Update healthbar and temp disable movement when taking damage
    public void TakeDamage(int damage, Transform origin = null)
    {
        if (health <= 0 || invuln) return;
        health -= damage;

        CameraShake.AddShake(0.2f); 

        if (health < 1)
        {
            CameraShake.AddShake(0.2f);
            Die();
        }
        else
        {
            DamageFlash();
            //StartCoroutine(DamageTimeout(true));
        }

        healthbar.SetHealth(health);
    }

    private void DamageFlash()
    {
        StartCoroutine(_DamageFlash());
    }

    IEnumerator _DamageFlash()
    {
        float currentTime = Time.time;
        invuln = true;
        while (Time.time - currentTime < recoveryTime)
        {
            yield return new WaitForSeconds(0.05f);
            sprite.color = sprite.color == origColor ? Color.grey : origColor;
        }
        sprite.color = origColor;
        invuln = false;
    }

    private void Die()
    {
        Destroy(gameObject);
        MissionManager.instance.objectiveComplete = true;
        MissionManager.instance.CompleteMission();
        campfire.SetActive(true);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damage, Vector2.zero);
        }   
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
