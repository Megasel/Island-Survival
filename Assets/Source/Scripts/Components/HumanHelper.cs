using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class HumanHelper : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float patrolSpeed = 2f;
    [SerializeField] float runSpeed = 4f;
    [SerializeField] float waypointRadius = 0.3f;
    [SerializeField] float obstacleAvoidanceDistance = 2f;
    [SerializeField] float idleTime;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] GameObject needFoodUI;

    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private Transform player;
    private bool isRunning = false;
    private bool idle;
    private HelperHireUI helperHireUI;
    public bool isHired { get; private set; }
    public Item hireItemCost;
    [SerializeField] Transform hireTargetPosition;
    public bool onShip;
    int objectID;
    private void Awake()
    {
        objectID = GetInstanceID();
    }

    public void LoadData()
    {
        HelperData objectData = DataManager.Instance.LoadHelperObjectData(objectID);
        if (objectData != null)
        {
            isHired = objectData.isHired;
            onShip=objectData.onShip;
            if (onShip)
            {
                gameObject.SetActive(false);
            }
            else if(isHired)
            {
                transform.position = hireTargetPosition.position;
                needFoodUI.SetActive(false);
            }
        }
    }

    public void SaveData()
    {
        DataManager.Instance.SaveHelperObjectData(objectID, isHired, onShip);
    }

    private void Start()
    {
        helperHireUI = FindObjectOfType<HelperHireUI>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isHired)
        {
            animator.SetBool("Run", false);
            idle = true;
            agent.speed = 0;
            transform.LookAt(other.transform.position);
            helperHireUI = FindObjectOfType<HelperHireUI>();
            helperHireUI.SetUI(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isHired)
        {
            idle = false;
            agent.speed = patrolSpeed;
            SetNextWaypoint();
            helperHireUI = FindObjectOfType<HelperHireUI>();
            helperHireUI.Close();
        }
    }

    public void Hire()
    {
        needFoodUI.SetActive(false);
        StopAllCoroutines();
        isHired = true;
        SaveData();
        Bootstrap.Instance.GameData.Inventory.ItemsInventory.Subtract(hireItemCost);
        Bootstrap.Instance.GameData.Inventory.InventoryScreen.UpdateUI(hireItemCost, Bootstrap.Instance.GameData.Inventory.ItemsInventory.Items.Find(x=>x.Id==hireItemCost.Id).Count);
        helperHireUI.Close();

        animator.SetBool("Run", true);
        agent.speed = runSpeed;
        agent.SetDestination(hireTargetPosition.position);
    }

    private void Update()
    {
        if (isHired && (agent.pathEndPosition-agent.transform.position).magnitude==0&&animator.GetBool("Run"))
        {
            animator.SetBool("Run", false);
        }
        if (isHired) return;

        if (!isHired&&!idle)
        {
            if (!agent.pathPending && agent.remainingDistance <= waypointRadius)
            {
                if (!idle)
                {
                    StartCoroutine(Idling());
                }
                else
                {
                    SetNextWaypoint();
                }
            }

            AvoidObstacles();
        }
    }

    private void SetNextWaypoint()
    {
        animator.SetBool("Run",true);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void AvoidObstacles()
    {
        Vector3 forward = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, obstacleAvoidanceDistance, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            Vector3 targetPosition = transform.position + avoidanceDirection * obstacleAvoidanceDistance;

            agent.SetDestination(targetPosition);
        }
    }

    IEnumerator Idling()
    {
        idle = true;
        animator.SetBool("Run",false);
        agent.speed = 0;
        yield return new WaitForSeconds(idleTime);
        SetNextWaypoint();
        agent.speed = patrolSpeed;
        idle = false;
    }
}

