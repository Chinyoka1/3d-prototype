using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private static readonly int Hash_Hit = Animator.StringToHash("Hit");
    private static readonly int Hash_Death = Animator.StringToHash("Death");

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 100;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider col)
    {
        Attack attack = col.gameObject.GetComponent<Attack>();

        if (attack != null)
        {
            TakeDamage(attack);
        }
    }

    private void TakeDamage(Attack attack)
    {
        if (health > 0)
        {
            health -= attack.GetFlatDamage();
        }

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        if (anim != null)
        {
            if (health > 0)
            {
                anim.SetTrigger(Hash_Hit);
            }
            else
            {
                anim.SetTrigger(Hash_Death);
            }
        }
    }
}