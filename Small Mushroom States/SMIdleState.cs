using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMIdleState : State
{
    bool sound;
    public SMIdleState() { StateName = StatesEnum.SMIdle; }


    float timer = 0f;
    public override void Enter()
    {
        sound = false;
        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");
        timer = 2f;
    }

    public override void Execute()
    {
        if (sound == false)
        {
            Object.FindObjectOfType<AudioManagerMush>().Play("Idle");
            sound = true;
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            AgentFSM.ChangeState(StatesEnum.SMPatrol);
        }

       // DetectPlayer();

    }

    public override void Exit()
    {

    }

    private void DetectPlayer()
    {
        if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 10)
        {
            RaycastHit hit;
            Vector3 direction = PlayerMovement.instance.transform.position - NavAgent.transform.position;
            //if (Physics.Raycast(NavAgent.transform.position, PlayerMovement.instance.transform.position - NavAgent.transform.position, out hit, 10f))
                if (Physics.SphereCast(NavAgent.transform.position, 0.34f, direction, out hit))

                {
                    if (hit.transform.tag == "Player")
                {
                    AgentFSM.ChangeState(StatesEnum.SMAttack);
                }
                else
                {
                    AgentFSM.ChangeState(StatesEnum.SMGetPlayerInSight);

                }
            }
        }
    }
}

