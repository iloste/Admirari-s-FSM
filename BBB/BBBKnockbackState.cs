using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBKnockbackState : State
{
    float timer;

    public BBBKnockbackState()
    {
        StateName = StatesEnum.BBBKnockback;
    }
    public override void Enter()
    {
    }

    public override void Execute()
    {
        AgentFSM.ChangeState(StatesEnum.BBBStateTransition);
    }

    public override void Exit()
    {
    }
}
