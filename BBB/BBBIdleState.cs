using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBIdleState : State
{
    float timeTillAttack;
    float attackTimer;
    public BBBIdleState()
    {
        StateName = StatesEnum.BBBIdle;
    }

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");

    }

    public override void Execute()
    {

        if (Vector3.Distance(PlayerMovement.instance.transform.position, AgentFSM.transform.position) < 4)
        {
            if (timeTillAttack > 0)
            {
                timeTillAttack -= Time.deltaTime;
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.BBBScythe);
                timeTillAttack = 3f;
            }
        }
       
    }

    public override void Exit()
    {
    }
}
