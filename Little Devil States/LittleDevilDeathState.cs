using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilDeathState : State
{
    float timer;
    bool playedParticles;
    bool sound;
    public LittleDevilDeathState()
    {
        StateName = StatesEnum.LittleDevilDeath;
    }

    public override void Enter()
    {
        sound = false;
        AgentFSM.Animator.SetInteger("State", 3);
        AgentFSM.Animator.SetTrigger("Trigger");
        timer = 1f;

        NavAgent.isStopped = true;
        NavAgent.ResetPath();
        playedParticles = false;
    }

    public override void Execute()
    {
        if (sound == false)
        {
            Object.FindObjectOfType<AudioManagerDevil>().Play("Death");
            sound = true;
        }
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
   
            if (timer < 0.2f && !playedParticles)
            {
                playedParticles = true;
                AgentFSM.transform.parent.GetComponent<EnemySpawner>().PlayParticleEffect();
                
            }
        }
        else
        {
            AgentFSM.GetComponentInParent<EnemySpawner>().AgentDead();
            //AgentFSM.ChangeState(StatesEnum.LittleDevilDance);
            AgentFSM.EnterStartState();
        }
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }
}
