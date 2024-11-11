using DG.Tweening;
using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float waypointRadius = 0.3f;
    [SerializeField] float obstacleAvoidanceDistance = 2f;
    [SerializeField] float idleTime=3f;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] Animator animator;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Transform player;
    private bool isRunning = false;
    private bool idle;

    private void Start()
    {
        animator=GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        SetNextWaypoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spear"))
        {
            StopAllCoroutines();
            DOVirtual.DelayedCall(0.4f, () => {
                idle = false;
                isRunning = true;
                agent.speed = runSpeed;
                animator.CrossFade("move", 0.1f, 0, 0);
                Vector3 runDirection = (transform.position - player.position).normalized;
                agent.SetDestination(transform.position + runDirection * 5);
            }).SetId(this.gameObject);
        }
    }

    private void Update()
    {
        if (/*!isRunning&&!idle&&*/!agent.pathPending && agent.remainingDistance <= waypointRadius)
        {
            if (!isRunning&&!idle)
            {
                StartCoroutine(Idling());
            }
            else 
            {
                SetNextWaypoint();
            }
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (isRunning)
        {
            if (distanceToPlayer > 6f)
            {
                isRunning = false;
                agent.speed = patrolSpeed;
                StartCoroutine(Idling());
                //SetNextWaypoint();
            }
        }
        // Avoid obstacles
        AvoidObstacles();
    }

    private void SetNextWaypoint()
    {
        currentWaypointIndex = Random.Range(0, waypoints.Length);// (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void AvoidObstacles()
    {
        Vector3 forward = transform.forward;
        RaycastHit hit;

        // Cast rays forward to detect obstacles
        if (Physics.Raycast(transform.position, forward, out hit, obstacleAvoidanceDistance, obstacleLayer))
        {
            // Calculate the new direction to avoid the obstacle
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            Vector3 targetPosition = transform.position + avoidanceDirection * obstacleAvoidanceDistance;

            // Move towards the target position
            agent.SetDestination(targetPosition);
        }
    }

    IEnumerator Idling()
    {
        idle = true;
        agent.speed = 0;
        animator.CrossFade("idle", 0.1f, 0, 0);
        yield return new WaitForSeconds(idleTime);
        SetNextWaypoint();
        agent.speed = patrolSpeed;
        animator.CrossFade("move", 0.1f, 0, 0);
        idle = false;
    }
}

