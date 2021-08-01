using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMSensorState : State
{
    public SMSensorState() { StateName = StatesEnum.SMSensor; }

    bool playerEnteredRange;

    public override void Enter()
    {
        playerEnteredRange = false;
    }

    public override void Execute()
    {

        if (AgentFSM.CurrentState.StateName != StatesEnum.SMAttack && AgentFSM.CurrentState.StateName != StatesEnum.SMDeath && AgentFSM.CurrentState.StateName != StatesEnum.SMHurt)
        {
            if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 10)
            {
                RaycastHit hit;
                Vector3 projectileSpawnPoint = (NavAgent.transform.position + ((Vector3.up) * 0.25f) - (NavAgent.transform.right * 0.5f));
                Vector3 direction = (PlayerMovement.instance.transform.position + (Vector3.up * 0.4f)) - projectileSpawnPoint;

                if (Physics.SphereCast(projectileSpawnPoint, 0.3f, direction, out hit))
                {
                    if (hit.transform.tag == "Player")
                    {
                        AgentFSM.ChangeState(StatesEnum.SMAttack);
                    }
                    else if (AgentFSM.CurrentState.StateName != StatesEnum.SMGetPlayerInSight)
                    {
                        AgentFSM.ChangeState(StatesEnum.SMGetPlayerInSight);
                    }

                }
            }
        }

       
    }

    public override void Exit()
    {
    }
}
