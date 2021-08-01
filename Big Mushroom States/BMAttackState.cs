using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMAttackState : State
{
    float spawnMushroomTimer;
    BMProjectileManager projectileManager;

    public BMAttackState()
    {
        StateName = StatesEnum.BMAttack;
    }

    public override void Enter()
    {
        // round y position to nearest .5f
        float y = (float)System.Math.Round(NavAgent.transform.position.y * 2) / 2;
        Vector3 position = new Vector3(NavAgent.transform.position.x, y, NavAgent.transform.position.z) - NavAgent.transform.right * 0.7f;

        if (projectileManager == null )
        {
            projectileManager = (Object.Instantiate(Resources.Load("Enemies/Big Mushroom/BM Projectile"), position, Quaternion.identity) as GameObject).GetComponent<BMProjectileManager>();
        }

        projectileManager.transform.position = position;

        AgentFSM.Animator.SetInteger("State", 3);
        AgentFSM.Animator.SetTrigger("Trigger");
        spawnMushroomTimer = 0.4f;
        projectileManager.FinishedAttack = false;
    }

    public override void Execute()
    {
        if (projectileManager.FinishedAttack)
        {
            // projectileManager.gameObject.SetActive(false);
            AgentFSM.ChangeState(StatesEnum.BMIdle);
        }
        else
        {
            if (spawnMushroomTimer > 0)
            {
                spawnMushroomTimer -= Time.deltaTime;
            }
            else
            {
                projectileManager.gameObject.SetActive(true);
            }
        }

       

      
    }

    public override void Exit()
    {
        if (projectileManager.gameObject.activeSelf)
        {
            projectileManager.StartCoroutine(projectileManager.Deactivate()); 
        }

    }

}
