using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearEarthquakeState : State
{
    float cooldownTimer;
    bool preparing;
    bool hitPlayer;

    public SkullBearEarthquakeState()
    {
        StateName = StatesEnum.SkullBearEarthquake;
    }

    public override void Enter()
    {
        // start animation?
        AgentFSM.Stats.CanBeDamaged = false;
        cooldownTimer = 0.6f;
        preparing = true;
        AgentFSM.Animator.SetInteger("State", 2);
        AgentFSM.Stats.CanBeDamaged = false;
        hitPlayer = false;
    }

    public override void Execute()
    {
        if (preparing)
        {
            if (cooldownTimer > 0)
            {
                // prepare for attack
                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                if (NavAgent.transform.position.x < PlayerMovement.instance.transform.position.x)
                {
                    AgentFSM.SpawnerParticles[0].Play();
                }
                else
                {
                    AgentFSM.SpawnerParticles[1].Play();
                    
                }

                CameraShake.instance.IncreaseTrauma(1.5f);
                preparing = false;
                cooldownTimer = 0.5f;
                Object.FindObjectOfType<AudioManager>().Play("BossAttack");
            }

        }
        else
        {
            if (cooldownTimer > 0)
            {
                AgentFSM.AgentParticles[0].Play();

                cooldownTimer -= Time.deltaTime;
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.SkullBearIdle);
            }
        }
    }


    public override void Exit()
    {

    }
}
