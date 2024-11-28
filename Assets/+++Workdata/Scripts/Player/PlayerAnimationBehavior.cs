using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehavior : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider, leftFootCollider, rightHandCollider, rightFootCollider, swordCollider;
    private PlayerController _playerController;
    private Animator anim;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    public void EndAttack()
    {
        _playerController.SetAnimator_UpperBody_LayerWeight(0);
    }

    public void ActivateRightHandCollider()
    {
        rightHandCollider.enabled = true;
    }
    
    public void DeactivateRightHandCollider()
    {
        rightHandCollider.enabled = false;
    }

    public void ActivateSwordCollider()
    {
        swordCollider.enabled = true;
    }
    
    public void DeactivateSwordCollider()
    {
        swordCollider.enabled = false;
    }
}
