using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimationBehaviour : MonoBehaviour
{
    private PlayerWeaponAbility _playerWeaponAbility;

    private void Awake()
    {
        _playerWeaponAbility = GetComponentInParent<PlayerWeaponAbility>();
    }
    
    public void WeaponSwitch()
    {
        _playerWeaponAbility.WeaponSwitch();
    }
}