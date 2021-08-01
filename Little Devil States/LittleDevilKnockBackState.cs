using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilKnockbackState : State
{
    float cooldownTimer;
    Vector3 direction;
    float speed;
    bool sound;


    public LittleDevilKnockbackState()
    {
        StateName = StatesEnum.LittleDevilKnockback;
    }

    public override void Enter()
    {
        cooldownTimer = 0.4f;
        speed = 0.35f;
        sound = false;

        direction = PlayerMovement.instance.transform.position - NavAgent.transform.position;
        direction.Normalize();

        NavAgent.SetDestination(AgentFSM.transform.position -= direction *2);
        NavAgent.speed = 7;

        AgentFSM.Animator.SetInteger("State", 2);
        AgentFSM.Animator.SetTrigger("Trigger");
    }


    public override void Execute()
    {

        if (sound == false)
        {
            Object.FindObjectOfType<AudioManagerDevil>().Play("GotHit");
            sound = true;
        }
        cooldownTimer -= Time.deltaTime;
        float dist = NavAgent.remainingDistance;
        // if (dist != Mathf.Infinity && NavAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
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
                    
                    AgentFSM.ChangeState(StatesEnum.LittleDevilDance2);
                }
            }
        }

            //if (speed != 0)
            //{
            //    if (speed < -0.2f)
            //    {
            //        speed += Time.deltaTime;
            //    }
            //    else if (speed > 0.2f)
            //    {
            //        speed -= Time.deltaTime;
            //    }
            //    else
            //    {
            //        speed = 0;
            //    }

            //    AgentFSM.transform.position -= direction * speed;
            //}
            //else
            //{
            //    if (cooldownTimer >= 0)
            //    {
            //        cooldownTimer -= Time.deltaTime;
            //    }
            //    else
            //    {
            //        // change state
            //        // if dead
            //        // change to death state
            //        // else
            //        // change to dance
            //        AgentFSM.Death();
            //    }
            //}
        }


    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
        NavAgent.speed = 3;
    }
}
