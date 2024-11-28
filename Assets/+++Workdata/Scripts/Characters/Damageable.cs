using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Damageable : MonoBehaviour
{
    private static readonly int Hash_Hit = Animator.StringToHash("Hit");
    private static readonly int Hash_Death = Animator.StringToHash("Death");

    public int maxHealth = 100;
    [SerializeField] private int health = 100;
    [SerializeField] private string reactOnTag = "Player";

    private Animator anim;

    public UnityEvent<int> onHealthChanged;

    private bool isDying;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public int GetHealth()
    {
        return health;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag(reactOnTag))
        {
            Attack attack = col.gameObject.GetComponent<Attack>();

            if (attack != null)
            {
                TakeDamage(attack);
            }
        }
    }

    private void TakeDamage(Attack attack)
    {
        if (health > 0)
        {
            health -= attack.GetFlatDamage();
        }
        
        if (health <= 0)
        {
            OnDeath();
        }

        PlayAnimation();
        onHealthChanged.Invoke(health);
    }

    public void TakeDamage(int damagePoints)
    {
        if (health > 0)
        {
            health -= damagePoints;
        }

        if (health <= 0)
        {
            OnDeath();
        }

        PlayAnimation();
        onHealthChanged.Invoke(health);
    }

    public void Heal(int healingPoints)
    {
        health += healingPoints;
        onHealthChanged.Invoke(health);
    }

    private void PlayAnimation()
    {
        if (anim != null)
        {
            if (health > 0)
            {
                anim.SetTrigger(Hash_Hit);
            }
        }
    }

    private void OnDeath()
    {
        if (!isDying)
        {
            anim.SetTrigger(Hash_Death);
        }
        isDying = true;
    }
}