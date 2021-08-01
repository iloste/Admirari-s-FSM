using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilIdleState : State
{
    public LittleDevilIdleState()
    {
        StateName = StatesEnum.LittleDevilIdle;
    }
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");
    }

    public override void Execute()
    {
    }

    public override void Exit()
    {
    }

 
}
