using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearStandState : State
{
    float timer;
    bool stamp1, stamp2;
    public SkullBearStandState()
    {
        StateName = StatesEnum.SkullBearStandState;
    }

    public override void Enter()
    {
        //stand animation
        AgentFSM.Animator.SetInteger("State", 6);

        timer = 2f;
        stamp1 = false;
        stamp2 = false;
    }

    public override void Execute()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (!stamp1 && timer < 0.85f)
            {
                CameraShake.instance.IncreaseTrauma(1f);
                stamp1 = true;
            }
            else if (!stamp2 && timer < 0.35f)
            {
                CameraShake.instance.IncreaseTrauma(1f);
                stamp2 = true;
            }

        }
        else
        {
            AgentFSM.ChangeState(StatesEnum.SkullBearRush);
        }
    }

    public override void Exit()
    {

    }
}
