using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOozeKnockbackState : State
{
    float cooldownTimer;
    Vector3 direction;


    public BigOozeKnockbackState()
    {
        StateName = StatesEnum.BigOozeKnockback;
    }

    public override void Enter()
    {
        cooldownTimer = 0.4f;
        


        direction = PlayerMovement.instance.transform.position - NavAgent.transform.position;
        direction.Normalize();

        NavAgent.SetDestination(AgentFSM.transform.position -= direction * 2);
        NavAgent.speed = 7;

        AgentFSM.Animator.SetInteger("State", 2);
        AgentFSM.Animator.SetTrigger("Trigger");
    }


    public override void Execute()
    {
        cooldownTimer -= Time.deltaTime;
        if (NavAgent.velocity.magnitude < 0.1)
        {
            if (cooldownTimer < 0)
            {
                if (AgentFSM.GetComponent<hsAI>().currentHealth <= 0)
                {
                    AgentFSM.Death();
                }
                else
                {
                    AgentFSM.ChangeState(StatesEnum.BigOozeMovement);
                }
            }
        }
    }


    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
        NavAgent.speed = 2;
    }
}
