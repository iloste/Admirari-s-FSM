using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilAttackState : State
{
    bool attacked;
    float timer;
    bool punched;
    bool sound;

    public LittleDevilAttackState()
    {
        StateName = StatesEnum.LittleDevilAttack;
    }

    public override void Enter()
    {
        attacked = false;
        timer = 0;
        punched = false;
        sound = false;
    }

    public override void Execute()
    {
        if (!attacked)
        {
            if (Vector3.Distance(AgentFSM.transform.position, PlayerMovement.instance.transform.position) <= 2)
            {
                //Object.FindObjectOfType<AudioManagerDevil>().Play("Prepare");
                AgentFSM.Animator.SetInteger("State", 5);
                AgentFSM.Animator.SetTrigger("Trigger");
                attacked = true;
                timer = 1.4f;
            }
            else
            {
              
                Vector3 direction = NavAgent.transform.position - PlayerMovement.instance.transform.position;
                direction.Normalize();
                NavAgent.SetDestination(PlayerMovement.instance.transform.position + direction * 0.5f);

                if (sound == false)
                {
                    Object.FindObjectOfType<AudioManagerDevil>().Play("Chase");
                    sound = true;
                }
            }
        }
        else
        {
            if (sound == false)
            {
                Object.FindObjectOfType<AudioManagerDevil>().Play("Chase");
                sound = true;
            }

            if (timer > 0)
            {
                
                timer -= Time.deltaTime;
                if (!punched && timer < 1f)
                {
                    punched = true;
                    Punch();
                }
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.LittleDevilDance2);
            }
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
        if (Physics.Raycast(AgentFSM.transform.position, -direction, out hit, 1.5f))
        {
            if (hit.transform.tag == "Player")
            {
                //////////////////////////////////////////////////////////////////Use stats damage///////////////////////////////////////////////////////////////////
                PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(2, NavAgent.transform.position, 0);

                PlayerMovement.instance.transform.GetComponent<hsPlayer>().Angle(NavAgent.transform.position);
            }
            else if (hit.transform.tag == "Shield")
            {
                PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(0, NavAgent.transform.position, 0);
            }
        }
    }
}
