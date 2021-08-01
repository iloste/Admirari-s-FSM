using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class SMPatrolState : State
{
    public SMPatrolState() { StateName = StatesEnum.SMPatrol; }


    private int amountOfWaypoints;
    private int nextWaypoint;
    bool checkedWaypoints = false;

    GameObject[] allWaypoints;
    WaypointObject[] waypointScript;


    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 1);
        AgentFSM.Animator.SetTrigger("Trigger");

        if (checkedWaypoints == false)
        {
            GetWaypoints();
        }

        SetDestination();
        NavAgent.isStopped = false;
    }

    public override void Execute()
    {
        float dist = NavAgent.remainingDistance;

        if (dist != Mathf.Infinity && NavAgent.pathStatus == NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
        {
            AgentFSM.ChangeState(StatesEnum.SMIdle);
        }
    }

    public override void Exit()
    {
    }

    private void SetDestination()
    {
        nextWaypoint = Random.Range(0, amountOfWaypoints);
        NavAgent.SetDestination(allWaypoints[nextWaypoint].transform.position);
    }


    private void GetWaypoints()
    {
        waypointScript = AgentFSM.transform.parent.GetComponentsInChildren<WaypointObject>();
        allWaypoints = new GameObject[waypointScript.Length];

        for (int idx = 0; idx < allWaypoints.Length; idx++)
        {
            allWaypoints[idx] = waypointScript[idx].gameObject;
        }

        amountOfWaypoints = allWaypoints.Length;
        checkedWaypoints = true;
    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 10)
        {
            RaycastHit hit;
            Vector3 direction = PlayerMovement.instance.transform.position - NavAgent.transform.position;
            //if (Physics.Raycast(NavAgent.transform.position, PlayerMovement.instance.transform.position - NavAgent.transform.position, out hit, 10f))
            if (Physics.SphereCast(NavAgent.transform.position, 0.34f, direction, out hit))

            {
                if (hit.transform.tag == "Player")
                {
                    AgentFSM.ChangeState(StatesEnum.SMAttack);
                }
                else
                {
                    AgentFSM.ChangeState(StatesEnum.SMGetPlayerInSight);

                }
            }
        }
    }

}
