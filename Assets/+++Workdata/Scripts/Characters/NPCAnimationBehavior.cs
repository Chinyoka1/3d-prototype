using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationBehavior : MonoBehaviour
{
    [SerializeField] private Collider leftHandCollider, leftFootCollider, rightHandCollider, rightFootCollider, swordCollider;
    private Animator anim;
    private Enemy _enemyController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        _enemyController = GetComponentInParent<Enemy>();
    }

    public void EndAttack()
    {
        _enemyController.EndAttack();
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

    public void Death()
    {
        
    }
}
