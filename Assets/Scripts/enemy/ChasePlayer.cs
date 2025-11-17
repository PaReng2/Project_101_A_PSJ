using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 50f;
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private float distancePlayer;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        distancePlayer = Vector3.Distance(transform.position, player.position);

        if (distancePlayer <= chaseRange)
        {
            Chase();
        }
        else
        {
            StopChasing();
        }

        if (distancePlayer <= attackRange)
        {
            Attack();
        }
    }

    void Chase()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
    }

    void StopChasing()
    {
        agent.isStopped=true;
    }

    void Attack()
    {
        agent.isStopped = true;
        transform.LookAt(player);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
