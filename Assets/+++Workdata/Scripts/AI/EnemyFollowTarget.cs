using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowTarget : MonoBehaviour
{
    private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");
    
    [SerializeField] private Animator anim;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float rotationDistance = 2;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private bool showGizmos = true;
    
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Vector3 originPosition;

    private Collider followTarget;

    private Enemy _enemyController;

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navMeshAgent.autoBraking = true;
        originPosition = transform.position;
        _enemyController = GetComponent<Enemy>();
    }

    private void Update()
    {
        anim.SetFloat(Hash_MovementSpeed, navMeshAgent.velocity.magnitude);
        
        if (followTarget != null && !navMeshAgent.isStopped)
        {
            navMeshAgent.destination = followTarget.transform.position;
            
            float distanceToTarget = Vector3.Distance(transform.position, followTarget.transform.position);
            
            if (distanceToTarget <= rotationDistance)
            {
                SmoothLookAtPlayer();
            }

            if (distanceToTarget <= attackDistance)
            {
                _enemyController.Attack();
            }
        }

        if (followTarget == null && !navMeshAgent.isStopped && transform.position == originPosition)
        {
            navMeshAgent.isStopped = true;
        }
    }

    public void StartFollowing(Collider col)
    {
        navMeshAgent.isStopped = false;
        followTarget = col;
        navMeshAgent.speed = chaseSpeed;
    }

    public void StopFollowing()
    {
        followTarget = null;
        navMeshAgent.destination = originPosition;
        navMeshAgent.speed = walkSpeed;
    }

    public void StopMovement()
    {
        navMeshAgent.isStopped = true;
    }

    public void ResumeMovement()
    {
        navMeshAgent.isStopped = false;
    }
    
    private void SmoothLookAtPlayer()
    {
        Vector3 direction = (followTarget.transform.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rotationDistance);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}
