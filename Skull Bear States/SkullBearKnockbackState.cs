using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearKnockbackState : State
{
    float timer;
    public SkullBearKnockbackState()
    {
        StateName = StatesEnum.SkullBearKnockback;
    }

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 3);
        timer = 0.5f;
        (AgentFSM.GetState(StatesEnum.SkullBearIdle) as SkullBearIdleState).Hurt++;
        AgentFSM.Stats.CanBeDamaged = false;
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (AgentFSM.GetComponent<hsAI>().currentHealth <= 0)
            {
                AgentFSM.Death();
            }
            else
            {
                AgentFSM.RevertState();
            }
        }
    }

    public override void Exit()
    {
    }
}
