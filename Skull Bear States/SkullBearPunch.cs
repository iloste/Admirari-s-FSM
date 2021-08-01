using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearPunch : State
{
    bool attacked;
    float timer;
    bool punched;

    public SkullBearPunch()
    {
        StateName = StatesEnum.SkullBearPunch;
    }
    public override void Enter()
    {
        timer = 0.4f;
        punched = false;
        AgentFSM.Animator.SetInteger("State", 5);

    }

    public override void Execute()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Punch();

            AgentFSM.ChangeState(StatesEnum.SkullBearRush);
        }
    }


    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }

    private void Punch()
    {
        RaycastHit hit;
        Vector3 direction = NavAgent.transform.position - PlayerMovement.instance.transform.position;
        direction.Normalize();


        if (Physics.SphereCast(AgentFSM.transform.position,0.3f,  -direction, out hit, 1.5f))
        {
            if (hit.transform.tag == "Player")
            {
                //////////////////////////////////////////////////////////////////Use stats damage///////////////////////////////////////////////////////////////////
                PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(2, NavAgent.transform.position, 0);
            }
            else if (hit.transform.tag == "Shield")
            {
                PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(0, NavAgent.transform.position, 0);
            }
        }
    }
}
