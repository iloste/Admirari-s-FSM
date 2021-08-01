using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMDeathState : State
{
    float timer;
    bool playedParticles;

    public BMDeathState()
    {
        StateName = StatesEnum.BMDeath;
    }
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 4);
        timer = 1.1f;
        AgentFSM.Stats.CanBeDamaged = false;
        playedParticles = false;

        SceneTransition.instance.endPos.gameObject.SetActive(true);
        SceneTransition.instance.beginPos.gameObject.SetActive(true);
    }

    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (timer < 0.5f && !playedParticles)
        {
            playedParticles = true;
            AgentFSM.transform.parent.GetComponent<EnemySpawner>().PlayParticleEffect();
        }

        if (timer <= 0)
        {
            AgentFSM.transform.parent.GetComponent<EnemySpawner>().AgentDead();
        }
    }

    public override void Exit()
    {
    }
}
