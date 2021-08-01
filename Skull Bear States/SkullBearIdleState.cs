using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearIdleState : State
{
    float timer;
    public int Hurt { get; set; }

    public SkullBearIdleState()
    {
        StateName = StatesEnum.SkullBearIdle;
    }


    public override void Enter()
    {
        timer = 9;
        AgentFSM.Animator.SetInteger("State", 0);
        Object.FindObjectOfType<AudioManager>().Play("Snore");
        AgentFSM.Stats.CanBeDamaged = true;

        

       
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
       

        if (timer <= 0 || Hurt >= 3)
        {
            Hurt = 0;
            
            AgentFSM.AgentParticles[0].Stop();
            
            AgentFSM.ChangeState(StatesEnum.SkullBearStandState);
        }

        if (timer <= 2f)
        {
            
            AgentFSM.AgentParticles[0].Stop();
            
        }

    }

    public override void Exit()
    {
        Object.FindObjectOfType<AudioManager>().Stop("Snore");
        AgentFSM.Stats.CanBeDamaged = false;
    }
}
