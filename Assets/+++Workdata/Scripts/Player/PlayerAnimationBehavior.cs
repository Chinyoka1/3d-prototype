using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehavior : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider, leftFootCollider, rightHandCollider, rightFootCollider;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
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
}
