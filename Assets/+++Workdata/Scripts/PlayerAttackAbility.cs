using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackAbility : MonoBehaviour
{
    private PlayerController _playerController;

    private InputAction attackAction;

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
        _playerController.SetAnimator_UpperBody_LayerWeight(1);
        _playerController.ActionTrigger(1);
    }
}
