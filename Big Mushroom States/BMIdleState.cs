using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMIdleState : State
{

    float timeTillAttack;
    float biteTimer;

    public BMIdleState()
    {
        StateName = StatesEnum.BMIdle;
    }
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");
        timeTillAttack = 5f;
    }

    public override void Execute()
    {
        if (Vector3.Distance(PlayerMovement.instance.transform.position, AgentFSM.transform.position) > 4)
        {
            if (timeTillAttack > 0)
            {
                timeTillAttack -= Time.deltaTime;
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.BMAttack);

            }
        }
        else
        {
            if (biteTimer > 0)
            {
                biteTimer -= Time.deltaTime;
            }
            else
            {
                biteTimer = 3;
                AgentFSM.ChangeState(StatesEnum.BMBite);
            }
        }

    }

    public override void Exit()
    {
    }
}
