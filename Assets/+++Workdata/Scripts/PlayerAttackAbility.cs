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
        PlayerController.EnableAbilities += EnableAbility;
        PlayerController.DisableAbilities += DisableAbility;
    }
    
    private void OnDisable()
    {
        PlayerController.EnableAbilities -= EnableAbility;
        PlayerController.DisableAbilities -= DisableAbility;
    }

    public void EnableAbility()
    {
        StartCoroutine(DelayAbility());
    }

    IEnumerator DelayAbility()
    {
        yield return null;
        attackAction = _playerController.inputActions.Player.Attack;
        attackAction.performed += AttackInput;
    }

    public void DisableAbility()
    {
        attackAction.performed -= AttackInput;
    }

    private void AttackInput(InputAction.CallbackContext ctx)
    {
        _playerController.SetAnimator_UpperBody_LayerWeight(1);
        _playerController.ActionTrigger(1);
    }
}
