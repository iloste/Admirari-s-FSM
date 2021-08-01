using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBFireBallState : State
{

    List<BBBProjectile> projectiles;
    float cooldownTimer;
    int projectile;
    float timerToInstantiate;
    bool fired;


    public BBBFireBallState()
    {
        StateName = StatesEnum.BBBFireBall;
    }

    public override void Enter()
    {

        if (projectiles == null)
        {
            projectiles = new List<BBBProjectile>();
        }


        AgentFSM.Animator.SetInteger("State", 1);
        AgentFSM.Animator.SetTrigger("Trigger");

        cooldownTimer = 4f;
        timerToInstantiate = 0.75f;
        fired = false;
        GetProjectile();
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
            // figure out which state to send it to
            AgentFSM.ChangeState(StatesEnum.BBBIdle);
        }

    }

    public override void Exit()
    {
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
            projectiles.Add(GameObject.Instantiate(Resources.Load<BBBProjectile>("Enemies/Big Bad Boss/BBB Projectile")));
            projectile = projectiles.Count - 1;
        }

        projectiles[projectile].transform.position = NavAgent.transform.position + (Vector3.up * 0.4f) + (AgentFSM.transform.forward * 0.25f) - (NavAgent.transform.right * 0.3f);
    }

}
