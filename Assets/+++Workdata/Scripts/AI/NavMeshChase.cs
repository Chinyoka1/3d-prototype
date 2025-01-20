using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshChase : MonoBehaviour
{
    private static readonly int Hash_MovementSpeed = Animator.StringToHash("MovementSpeed");

    private enum PreferredPosition
    {
        Random,
        Front,
        Back,
        Flank
    }

    [SerializeField] private Animator anim;
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float walkSpeed = 2.5f;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private float rotationDistance = 2;
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private float positioningDistance = 3;
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private PreferredPosition preferredPosition;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Vector3 originPosition;

    private Collider followTarget;

    private Enemy _enemyController;
    private bool hasPositioningPoint;

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

        if (followTarget != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, followTarget.transform.position);

            if (!navMeshAgent.isStopped)
            {
                if (!hasPositioningPoint)
                {
                    navMeshAgent.destination = followTarget.transform.position;
                }
            }

            if (distanceToTarget <= rotationDistance)
            {
                SmoothLookAtPlayer();
            }

            if (distanceToTarget <= positioningDistance && !hasPositioningPoint)
            {
                hasPositioningPoint = true;
                PlayerPositioningBehaviour positioningBehaviour =
                    followTarget.GetComponent<PlayerPositioningBehaviour>();

                switch (preferredPosition)
                {
                    case PreferredPosition.Random:
                    {
                        navMeshAgent.destination = positioningBehaviour.GetRandomPositioningPoint().position;
                        break;
                    }
                    case PreferredPosition.Back:
                    {
                        navMeshAgent.destination = positioningBehaviour.GetPositioningPointBack().position;
                        break;
                    }
                    case PreferredPosition.Front:
                    {
                        navMeshAgent.destination = positioningBehaviour.GetPositioningPointFront().position;
                        break;
                    }
                    case PreferredPosition.Flank:
                    {
                        navMeshAgent.destination = positioningBehaviour.GetPositioningPointFlanks().position;
                        break;
                    }
                }
            }
            else if (distanceToTarget > positioningDistance && hasPositioningPoint)
            {
                hasPositioningPoint = false;
            }

            if (distanceToTarget <= attackDistance)
            {
                _enemyController.Attack();
            }
        }
    }

    public void StartFollowing(Collider col)
    {
        followTarget = col;
        navMeshAgent.speed = chaseSpeed;
        GetComponent<NavMeshPatrol>().enabled = false;
    }

    public void StopFollowing()
    {
        followTarget = null;
        navMeshAgent.destination = originPosition;
        navMeshAgent.speed = walkSpeed;
        GetComponent<NavMeshPatrol>().enabled = true;
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
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, positioningDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, rotationDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
}