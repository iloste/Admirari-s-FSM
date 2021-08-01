using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMDeathState : State
{
    bool sound;
    public SMDeathState()
    {
        StateName = StatesEnum.SMDeath;
    }

    float timer;

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 3);
        sound = false;
        timer = 1f;

        NavAgent.isStopped = true;
        NavAgent.ResetPath();

    }

    public override void Execute()
    {
        if (sound == false)
        {
            Object.FindObjectOfType<AudioManagerMush>().Play("Die");
            sound = true;
        }

        if (timer > 0)
        {
           
            timer -= Time.deltaTime;
            if (timer < 0.5f)
            {
                AgentFSM.transform.parent.GetComponent<EnemySpawner>().PlayParticleEffect();
            }
        }
        else
        {
            if (BMSMTracker.instance)
            {
                AgentFSM.GetComponentInChildren<ParticleConnection>().Deactivate();
                BMSMTracker.instance.MushroomKilled(); 
            }
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
