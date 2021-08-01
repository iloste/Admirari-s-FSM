using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMBite : State
{
    float timer;
    bool attacked;
    public BMBite()
    {
        StateName = StatesEnum.BMBite;
    }

    public override void Enter()
    {
        //start animation
        AgentFSM.Animator.SetInteger("State", 5);
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
                AgentFSM.ChangeState(StatesEnum.BMIdle);
                // change to cage player state;
            }
        }
        // wait for animation to finish
        // change state
    }

    public override void Exit()
    {

    }
}
