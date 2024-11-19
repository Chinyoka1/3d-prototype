using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int damage;

    public int GetFlatDamage()
    {
        return damage;
    }
}
