using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMEscapeState : State
{
    float timer;
    public SMEscapeState()
    {
        StateName = StatesEnum.SMEscape;
    }
    public override void Enter()
    {
        timer = 5;
    }

    public override void Execute()
    {

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            AgentFSM.ChangeState(StatesEnum.SMAttack);
        }
    }

    public override void Exit()
    {
    }
}
