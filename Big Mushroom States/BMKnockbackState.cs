using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMKnockbackState : State
{
    int transitionCount;
    float animationTimer;
    public BMKnockbackState()
    {
        StateName = StatesEnum.BMKnockback;
    }
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 4);
        AgentFSM.Animator.SetTrigger("Trigger");
        animationTimer = 0.5f;
        AgentFSM.GetComponent<BoxCollider>().enabled = false;
    }

    public override void Execute()
    {
        if (animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;
        }
        else
        {
            if (transitionCount < 2)
            {
                transitionCount++;
                AgentFSM.ChangeState(StatesEnum.BMStageTransition);
            }
            else
            {
                AgentFSM.Death();
            }
        }
    }

    public override void Exit()
    {

    }

   
}
