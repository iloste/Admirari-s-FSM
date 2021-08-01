using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOozeIdleState : State
{
    public BigOozeIdleState() { StateName = StatesEnum.BigOozeIdle; }

    float timer = 0f;
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");
        timer = 2f;
    }

    public override void Execute()
    {

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            AgentFSM.ChangeState(StatesEnum.BigOozeMovement);
        }
    }

    public override void Exit()
    {

    }
}
