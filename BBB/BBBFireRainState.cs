using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBFireRainState : State
{

    public BBBFireRainState()
    {
        StateName = StatesEnum.BBBFireRain;
    }
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 3);
        AgentFSM.Animator.SetTrigger("Trigger");


        FireRainManager.instance.RainFire();

    }

    public override void Execute()
    {
        if (FireRainManager.instance.Spawn == false)
        {
            AgentFSM.ChangeState(StatesEnum.BBBIdle);
        }
    }

    public override void Exit()
    {
    }
}
