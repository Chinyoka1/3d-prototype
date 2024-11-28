using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehavior : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider,
        leftFootCollider,
        rightHandCollider,
        rightFootCollider,
        swordAttack1Collider,
        swordAttack2Collider,
        swordAttack3Collider;

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
        swordAttack1Collider.enabled = true;
    }

    public void DeactivateSwordCollider()
    {
        swordAttack1Collider.enabled = false;
    }
    public void ActivateSwordCollider2()
    {
        swordAttack2Collider.enabled = true;
    }

    public void DeactivateSwordCollider2()
    {
        swordAttack2Collider.enabled = false;
    }
    public void ActivateSwordCollider3()
    {
        swordAttack3Collider.enabled = true;
    }

    public void DeactivateSwordCollider3()
    {
        swordAttack3Collider.enabled = false;
    }

    public void Death()
    {
        FindObjectOfType<UIManager>().EnableDeathScreen();
    }
}