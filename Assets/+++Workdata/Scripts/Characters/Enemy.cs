using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Camera mainCam;
    private GameObject worldSpaceCanvas;
    [SerializeField] private Animator anim;
    private static readonly int Hash_ActionId = Animator.StringToHash("ActionId");
    private static readonly int Hash_ActionTrigger = Animator.StringToHash("ActionTrigger");
    private int upperBody_AnimLayer;
    private EnemyFollowTarget _enemyFollowTarget;
    private bool isAttacking;
    [SerializeField] private float attackDelay = 2;
    private Damageable _damageable;
    
    private void Awake()
    {
        mainCam = Camera.main;
        worldSpaceCanvas = GetComponentInChildren<Canvas>().gameObject;
        upperBody_AnimLayer = anim.GetLayerIndex("Upper Body");
        _enemyFollowTarget = GetComponent<EnemyFollowTarget>();
        _damageable = GetComponentInChildren<Damageable>();
        _damageable.onDeath.AddListener(OnDeath);
    }

    private void Update()
    {
        worldSpaceCanvas.transform.rotation = mainCam.transform.rotation;
    }

    private void OnDeath()
    {
        _enemyFollowTarget.StopMovement();
        worldSpaceCanvas.SetActive(false);
    }
    
    public void SetAnimator_UpperBody_LayerWeight(float weight)
    {
        anim.SetLayerWeight(upperBody_AnimLayer, weight);
    }
    
    public void ActionTrigger(int id)
    {
        anim.SetTrigger(Hash_ActionTrigger);
        anim.SetInteger(Hash_ActionId, id);
    }

    public void Attack()
    {
        if (!isAttacking && _damageable.isAlive)
        {
            isAttacking = true;
            SetAnimator_UpperBody_LayerWeight(1);
            ActionTrigger(1);

            if (_enemyFollowTarget)
            {
                _enemyFollowTarget.StopMovement();
            }
        }
    }

    public void EndAttack()
    {
        SetAnimator_UpperBody_LayerWeight(0);
        if (_enemyFollowTarget && _damageable.isAlive)
        {
            _enemyFollowTarget.ResumeMovement();
        }

        StartCoroutine(DelayAttack());
    }

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
}
