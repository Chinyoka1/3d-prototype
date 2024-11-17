using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int health = 100;

    private void OnTriggerEnter(Collider col)
    {
        print("enter");
        Attack attack = col.gameObject.GetComponent<Attack>();
        if (attack != null)
        {
            health -= attack.GetDamage();
        }
    }
}
