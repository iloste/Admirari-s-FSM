using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SMGetPlayerInSightState : State
{
    public SMGetPlayerInSightState() { StateName = StatesEnum.SMGetPlayerInSight; }


    WaypointObject[] lookoutPoints;
    int destination;

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 1);
        AgentFSM.Animator.SetTrigger("Trigger");

        destination = -1;

        // get lookout points if hasn't already
        if (lookoutPoints == null || lookoutPoints.Length == 0)
        {
            lookoutPoints = AgentFSM.transform.parent.GetComponentsInChildren<WaypointObject>();

            if (lookoutPoints.Length == 0)
            {
                Debug.LogError(AgentFSM.transform.name + " has no lookout points");
            }

        }

        destination = GetDestination();
        if (destination == -1)
        {

            destination = Random.Range(0, lookoutPoints.Length - 1);
        }
        else
        {
            NavAgent.SetDestination(lookoutPoints[destination].transform.position);

        }

        NavAgent.isStopped = false;

    }

    public override void Execute()
    {
        float dist = NavAgent.remainingDistance;

        if (dist != Mathf.Infinity && NavAgent.pathStatus == NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
        {
            AgentFSM.ChangeState(StatesEnum.SMAttack);
        }

      
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
    }


    /// <summary>
    /// returns the index of the lookout point that is furthest away from the player but from which the player can still be seen.
    /// </summary>
    /// <returns></returns>
    private int GetDestination()
    {
        RaycastHit hit;
        int destination = -1;

        for (int i = 0; i < lookoutPoints.Length; i++)
        {
            Vector3 projectileSpawnPoint = new Vector3(lookoutPoints[i].transform.position.x, NavAgent.transform.position.y + 0.4f, lookoutPoints[i].transform.position.z);

            Vector3 direction = PlayerMovement.instance.transform.position - projectileSpawnPoint;

            if (Physics.SphereCast(projectileSpawnPoint, 0.3f, direction, out hit))
            {
                if (hit.transform.tag == "Player")
                {
                    lookoutPoints[i].DistanceToPlayer = Vector3.Distance(lookoutPoints[i].transform.position, PlayerMovement.instance.transform.position);

                    if (destination != -1)
                    {
                        if (lookoutPoints[i].DistanceToPlayer > lookoutPoints[destination].DistanceToPlayer)
                        {
                            destination = i;
                        }
                    }
                    else
                    {
                        destination = i;
                    }
                }
            }
        }

        return destination;
    }

}
