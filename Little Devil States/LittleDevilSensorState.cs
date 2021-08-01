using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilSensorState : State
{
    bool playerEnteredRange;

    public LittleDevilSensorState()
    {
        StateName = StatesEnum.LittleDevilSensor;
    }
    public override void Enter()
    {
        playerEnteredRange = false;
    }

    public override void Execute()
    {

        if (AgentFSM.CurrentState.StateName != StatesEnum.LittleDevilKnockback && AgentFSM.CurrentState.StateName != StatesEnum.LittleDevilDeath)
        {
            // if on same level
            if (Mathf.Abs(NavAgent.transform.position.y - PlayerMovement.instance.GroundedY) < 0.5f)
            {
                // if near
                if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 7)
                {
                    // if player hasn't entered range yet, set entered true
                    if (!playerEnteredRange)
                    {
                        playerEnteredRange = true;
                        AgentFSM.ChangeState(StatesEnum.LittleDevilDance2);
                    }
                }
                else
                {
                    // if player has entered range already, set entered false
                    if (playerEnteredRange)
                    {
                        playerEnteredRange = false;
                        AgentFSM.ChangeState(StatesEnum.LittleDevilPatrol);
                    }
                    
                }
            }
            else
            {
                // if not exited yet, set entered false
                if (playerEnteredRange)
                {
                    playerEnteredRange = false;
                    AgentFSM.ChangeState(StatesEnum.LittleDevilPatrol);
                }
            }
        }
    }

    public override void Exit()
    {
    }
}
