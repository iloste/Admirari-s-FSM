using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOozeAttackState : State
{
    bool attacked;
    float attackTimer;
    float idleTimer;
    bool punched;

    public BigOozeAttackState()
    {
        StateName = StatesEnum.BigOozeAttack;
    }
    public override void Enter()
    {
        attacked = false;
        attackTimer = 0;
        punched = false;
        idleTimer = 0.5f;
    }

    public override void Execute()
    {
        if (!attacked)
        {
            // If play is in attack range
            if (Vector3.Distance(AgentFSM.transform.position, PlayerMovement.instance.transform.position) <= 2)
            {
                // play attack animation
                AgentFSM.Animator.SetInteger("State", 4);
                AgentFSM.Animator.SetTrigger("Trigger");
                attacked = true;
                attackTimer = 0.5f;
            }
            // move to player
            else
            {
                Vector3 direction = NavAgent.transform.position - PlayerMovement.instance.transform.position;
                direction.Normalize();
                NavAgent.SetDestination(PlayerMovement.instance.transform.position + direction * 0.5f);
            }
        }
        else
        {
            // counting down till end of attack animation
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;

                if (!punched && attackTimer <= 0)
                {
                    Object.FindObjectOfType<AudioManagerMobs>().Play("Ooze_Attack");
                    // Try to deal damage
                    punched = true;
                    Punch();

                    // set animation to idle
                    AgentFSM.Animator.SetInteger("State", 0);
                    AgentFSM.Animator.SetTrigger("Trigger");
                }
            }
            else if (idleTimer > 0)
            {
                idleTimer -= Time.deltaTime;
            }
            else if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 5)
            {
                AgentFSM.ChangeState(StatesEnum.BigOozeChase);
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.BigOozeMovement);
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

        if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 1.3f)
        {
            PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(1, NavAgent.transform.position, 0);
            
        }
        else if (Physics.Raycast(AgentFSM.transform.position - direction, -direction, out hit, 1f))
        {
            if (hit.transform.tag == "Player")
            {
                //////////////////////////////////////////////////////////////////Use stats damage///////////////////////////////////////////////////////////////////
                PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamage(1, NavAgent.transform.position, 0);
            }
        }

    }


}
