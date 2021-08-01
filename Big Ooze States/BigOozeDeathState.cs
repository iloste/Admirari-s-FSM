using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOozeDeathState : State
{
    float timer;
    bool Sound=false;
    public BigOozeDeathState()
    {
        StateName = StatesEnum.BigOozeDeath;
    }

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 3);
        
        timer = 1f;

        NavAgent.isStopped = true;
        NavAgent.ResetPath();

    }

    public override void Execute()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if(timer<0.6f&&Sound==false)
            {
                Object.FindObjectOfType<AudioManagerMobs>().Play("Ooze_Death");
                Sound = true;
            }
            if (timer < 0.5f)
            {
                AgentFSM.transform.parent.GetComponent<EnemySpawner>().PlayParticleEffect();
            }
        }
        else
        {
            AgentFSM.GetComponentInParent<EnemySpawner>().AgentDead();
            AgentFSM.EnterStartState();
        }
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }

}
