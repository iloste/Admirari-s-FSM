using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigOozeMovementState : State
{
    public BigOozeMovementState() { StateName = StatesEnum.BigOozeMovement; }
    bool sound;
    private int amountOfWaypoints;
    private int nextWaypoint;
    bool checkWaypoints = false;

    GameObject[] allWaypoints;
    WaypointObject[] waypointScript;

    public override void Enter()
    {
        sound = false;
        AgentFSM.Animator.SetInteger("State", 1);
        AgentFSM.Animator.SetTrigger("Trigger");

        
        if (checkWaypoints == false)
        {
            GetWaypoints();
        }

        SetDestination();
    }

    public override void Execute()
    {
        
        float dist = NavAgent.remainingDistance;

        if (dist != Mathf.Infinity && NavAgent.pathStatus == NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
        {
            
            AgentFSM.ChangeState(StatesEnum.BigOozeIdle);
        }
        
    }

    public override void Exit()
    {
    }

    private void SetDestination()
    {
        if (sound == false)
        {
            Object.FindObjectOfType<AudioManagerMobs>().Play("Ooze_Movement");
            sound = true;
        }
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
        checkWaypoints = true;
    }
}
