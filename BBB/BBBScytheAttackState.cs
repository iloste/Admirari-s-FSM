using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBScytheAttackState : State
{

    float timer;
    bool attacked;


    public BBBScytheAttackState()
    {
        StateName = StatesEnum.BBBScythe;
    }
    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 2);
        AgentFSM.Animator.SetTrigger("Trigger");
        timer = 0.45f;
        attacked = false;
    }

    public override void Execute()
    {
        if (!attacked)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                // deal damage
                attacked = true;
                timer = 0.45f;

                if (Vector3.Distance(NavAgent.transform.position, PlayerMovement.instance.transform.position) < 3)
                {
                    PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(1, NavAgent.transform.position, 0);
                }
            }
        }
        else
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.BBBIdle);
                // change to cage player state;
            }
        }
    }

    public override void Exit()
    {
    }
}
