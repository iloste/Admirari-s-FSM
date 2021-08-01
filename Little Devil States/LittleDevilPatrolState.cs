using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LittleDevilPatrolState : State
{
    float idleTimer;
    float maxIdleTime = 2;
    bool setDestination;
    int patrolDistance;

    public LittleDevilPatrolState()
    {
        StateName = StatesEnum.LittleDevilPatrol;
    }

    public override void Enter()
    {
        idleTimer = maxIdleTime;
        setDestination = false;
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");
        patrolDistance = 2;
    }

    public override void Execute()
    {
        if (!setDestination)
        {
            if (idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            }
            else
            {
                idleTimer = maxIdleTime;
                setDestination = true;
                NavAgent.SetDestination(GetDestination());
                AgentFSM.Animator.SetInteger("State", 1);
                AgentFSM.Animator.SetTrigger("Trigger");
            }
        }
        else
        {
            if (ReachedDestination())
            {
                setDestination = false;
                AgentFSM.Animator.SetInteger("State", 0);
                AgentFSM.Animator.SetTrigger("Trigger");
            }
        }
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }

    private bool ReachedDestination()
    {
        float dist = NavAgent.remainingDistance;
        if (dist != Mathf.Infinity && NavAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 GetDestination()
    {
        List<int> directions = new List<int>() { 0, 1, 2, 3 };
        NavMeshPath path = new NavMeshPath();
        Vector3 destination;

        do
        {
            int rand = Random.Range(0, directions.Count);
            switch (directions[rand])
            {
                case 0:
                    destination = NavAgent.transform.position + new Vector3(patrolDistance, 0, 0);
                    break;
                case 1:
                    destination = NavAgent.transform.position - new Vector3(patrolDistance, 0, 0);
                    break;
                case 2:
                    destination = NavAgent.transform.position + new Vector3(0, 0, patrolDistance);
                    break;
                case 3:
                    destination = NavAgent.transform.position - new Vector3(0, 0, patrolDistance);
                    break;
                default:
                    destination = NavAgent.transform.position;
                    break;
            }

            directions.RemoveAt(rand);
            NavMesh.CalculatePath(NavAgent.transform.position, destination, NavMesh.AllAreas, path);

        } while (directions.Count > 0 && path.status != NavMeshPathStatus.PathComplete);

        return destination;
    }
}
