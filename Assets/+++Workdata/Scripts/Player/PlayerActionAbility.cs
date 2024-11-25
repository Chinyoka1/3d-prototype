using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActionAbility : MonoBehaviour
{
    private PlayerController _playerController;

    private InputAction attackAction;

    public int actionId = 1;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }


    private void OnEnable()
    {
        StartCoroutine(DelayAbility());
    }
    
    private void OnDisable()
    {
        attackAction.performed -= AttackInput;
    }

    IEnumerator DelayAbility()
    {
        yield return null;
        attackAction = _playerController.inputActions.Player.Attack;
        attackAction.performed += AttackInput;
    }

    private void AttackInput(InputAction.CallbackContext ctx)
    {
        PlayerWeaponAbility playerWeaponAbility = GetComponent<PlayerWeaponAbility>();

        if (playerWeaponAbility)
        {
            if (playerWeaponAbility.weaponState == PlayerWeaponAbility.WeaponState.TwoHandSword)
            {
                _playerController.ActionTrigger(10);
            } 
            else if (playerWeaponAbility.weaponState == PlayerWeaponAbility.WeaponState.Unarmed)
            {
                _playerController.SetAnimator_UpperBody_LayerWeight(1);
                _playerController.ActionTrigger(1);
            }
        }
        else
        {
            _playerController.SetAnimator_UpperBody_LayerWeight(1);
            _playerController.ActionTrigger(1);
        }
    }
}
