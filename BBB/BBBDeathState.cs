using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBDeathState : State
{
    float timer;
    bool playedParticles;
    bool enteredDeathAnimation = false;
    bool audioFinished = false;
    public BBBDeathState()
    {
        StateName = StatesEnum.BBBDeath;
    }
    public override void Enter()
    {
        Object.FindObjectOfType<AudioManager>().Play("Boss_Final_Dialogue");
        // idle
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");

    }

    public override void Execute()
    {
        if (Object.FindObjectOfType<AudioManager>().IsPlaying("Boss_Final_Dialogue")==false)
        {
            if (!enteredDeathAnimation)
            {
                enteredDeathAnimation = true;
                EnterDeathAnimation();
            }
            DeathTimer();

        }
    }

    private void EnterDeathAnimation()
    {
        AgentFSM.Animator.SetInteger("State", 6);
        AgentFSM.Animator.SetTrigger("Trigger");
        timer = 1.1f;
        playedParticles = false;
    }

    private void DeathTimer()
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
            SceneTransition.instance.endPos.gameObject.SetActive(true);
            SceneTransition.instance.beginPos.gameObject.SetActive(true);
        }
    }

    public override void Exit()
    {
    }
}
