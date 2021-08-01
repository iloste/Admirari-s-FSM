using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOozeSensorState : State
{
    bool playerEnteredRange;

    public BigOozeSensorState()
    {
        StateName = StatesEnum.BigOozeSensor;
    }
    public override void Enter()
    {
        playerEnteredRange = false;
    }

    public override void Execute()
    {

        if (AgentFSM.CurrentState.StateName != StatesEnum.BigOozeKnockback && AgentFSM.CurrentState.StateName != StatesEnum.BigOozeDeath)
        {
            // if on same level
            float x = (Mathf.Abs(NavAgent.transform.position.y - PlayerMovement.instance.GroundedY));
            if (Mathf.Abs(NavAgent.transform.position.y - PlayerMovement.instance.GroundedY) < 0.8f)
            {
                // if near
                if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 5)
                {
                    // if player hasn't entered range yet, set entered true
                    if (!playerEnteredRange)
                    {
                        playerEnteredRange = true;
                        AgentFSM.ChangeState(StatesEnum.BigOozeChase);//change to chase state when done
                    }
                }
                else
                {
                    // if player has entered range already, set entered false
                    if (playerEnteredRange)
                    {
                        playerEnteredRange = false;
                        AgentFSM.ChangeState(StatesEnum.BigOozeIdle);
                    }

                }
            }
            else
            {
                // if not exited yet, set entered false
                if (playerEnteredRange)
                {
                    playerEnteredRange = false;
                    AgentFSM.ChangeState(StatesEnum.BigOozeIdle);
                }
            }
        }
    }

    public override void Exit()
    {
    }
}
