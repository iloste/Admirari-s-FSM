using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMAttackState : State
{
    List<SMProjectile> projectiles;
    float cooldownTimer;
    int projectile;
    float timerToInstantiate;
    bool fired;
    bool isIdle;


    public SMAttackState()
    {
        StateName = StatesEnum.SMAttack;
    }

    public override void Enter()
    {
        if (projectiles == null)
        {
            projectiles = new List<SMProjectile>();
        }

        SetUp();
    }



    public override void Execute()
    {
        if (timerToInstantiate > 0)
        {
            timerToInstantiate -= Time.deltaTime;
        }
        else
        {
            if (!fired)
            {
                fired = true;
                projectiles[projectile].Activate();
            }
        }

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 10)
            {
                AgentFSM.ChangeState(StatesEnum.SMGetPlayerInSight);
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.SMIdle);
            }
        }


    }

    public override void Exit()
    {
    }

    // called on enter 
    private void SetUp()
    {
       
        Object.FindObjectOfType<AudioManagerMush>().Play("Charge");
        AgentFSM.Animator.SetInteger("State", 4);
        AgentFSM.Animator.SetTrigger("Trigger");
        cooldownTimer = 4f;
        timerToInstantiate = 0.75f;
        fired = false;
        GetProjectile();
    }

    private void GetProjectile()
    {
        projectile = -1;

        // find available projectile
        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles[i].Active == false)
            {
                projectile = i;
                break;
            }
        }

        // if no projectile exists
        if (projectile == -1)
        {
            projectiles.Add(GameObject.Instantiate(Resources.Load<SMProjectile>("Enemies/Small Mushroom/SM Projectile")));
            projectile = projectiles.Count - 1;
        }

        projectiles[projectile].transform.position = NavAgent.transform.position + (Vector3.up * 0.4f) + (AgentFSM.transform.forward * 0.25f) - (NavAgent.transform.right * 0.3f);
        
      
            Object.FindObjectOfType<AudioManagerMush>().Play("Attack");
           
        
    }

}
