using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearDeathState : State
{

    bool HasGrunted=false;
    float timer;
    bool playedParticles;
    public SkullBearDeathState()
    {
        StateName = StatesEnum.SkullBearDeath;
    }

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 4);
        timer = 3;
        AgentFSM.Stats.CanBeDamaged = false;
        playedParticles = false;
        SceneTransition.instance.endPos.gameObject.SetActive(true);
    }


    public override void Execute()
    {
        timer -= Time.deltaTime;
        if (timer<2f&& HasGrunted==false)
        {
            Object.FindObjectOfType<AudioManager>().Play("Boss_Death");
            HasGrunted = true;
        }
        if (timer < 0.5f && !playedParticles)
        {
            
            playedParticles = true;
            AgentFSM.transform.parent.GetComponent<EnemySpawner>().PlayParticleEffect();
        }

        if (timer <= 0)
        {
            //Object.FindObjectOfType<BossDrop>().DropFunc(this.NavAgent);
            AgentFSM.transform.parent.GetComponent<EnemySpawner>().AgentDead();
        }
    }

    public override void Exit()
    {
    }
}
