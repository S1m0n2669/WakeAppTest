using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Осуществляет управление ботом
/// </summary>
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(AvatarController))]
public class AIController : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent Agent { get; private set; }
    public AvatarController Avatar { get; private set; }

    public PatrolPath CurrentPatrolPath = null;

    Transform currentTarget;
    int currentPatrolNumber = 0;
    bool ReverceMode; //если true, значит идём по контрольным точкам в обратном направлении

    private void Start()
    {
        Agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        Avatar = GetComponent<AvatarController>();

        Agent.updateRotation = false;
        Agent.updatePosition = true;

        if (CurrentPatrolPath != null && CurrentPatrolPath.PathPointList != null && CurrentPatrolPath.PathPointList.Length > 0)
        {
            SetTarget(CurrentPatrolPath.PathPointList[0]);
            Agent.SetDestination(currentTarget.position);
        }
        else
        {
            Debug.LogWarning("No any PatrolPath for Bot: " + this.name);
        }
    }

    private void Update()
    {
        if (Agent.remainingDistance > Agent.stoppingDistance)
        {
            Avatar.Move(Agent.desiredVelocity);
            Avatar.RotateTo(Agent.desiredVelocity);
        }
        else
        {
            Avatar.Move(Vector3.zero);
            SetNextPatrolPoint();

            if (currentTarget != null)
                Agent.SetDestination(currentTarget.position);
        }
    }

    void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    /// <summary>
    /// Реализует иттератор по контрольным точкам - "туда-обратно"
    /// </summary>
    void SetNextPatrolPoint()
    {
        if (!ReverceMode)
        {
            if (currentPatrolNumber + 1 >= CurrentPatrolPath.PathPointList.Length)
            {
                currentPatrolNumber--;
                ReverceMode = true;
            }
            else
            {
                currentPatrolNumber++;
            }
        }
        else
        {
            if (currentPatrolNumber - 1 < 0)
            {
                currentPatrolNumber++;
                ReverceMode = false;
            }
            else
            {
                currentPatrolNumber--;
            }
        }

        SetTarget(CurrentPatrolPath.PathPointList[currentPatrolNumber]);
    }
}
