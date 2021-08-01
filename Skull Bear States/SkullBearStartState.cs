using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearStartState : State
{
    bool HasGrunted = false;
    float timer;
    public SkullBearStartState()
    {
        StateName = StatesEnum.SkullBearStartState;
    }
    public override void Enter()
    {
       
        NavAgent.transform.position = AgentFSM.StartPos + new Vector3(13, 0, 0);
        timer = 1.3f;
        AgentFSM.Stats.CanBeDamaged = false;
        AgentFSM.FacePlayerBool = true;
    }

    public override void Execute()
    {
       
        if (Vector3.Distance(NavAgent.transform.position, PlayerMovement.instance.transform.position) < 19)
        {
            
            timer -= Time.deltaTime;
            if(timer<=1.3f&&timer>0.5)
            {
                if (HasGrunted == false)
                {
                    Object.FindObjectOfType<AudioManager>().Play("Boss_Start");
                    HasGrunted = true;
                }
                
            }
            if (timer <= 0)
            {
                AgentFSM.ChangeState(StatesEnum.SkullBearRush);
            }
        }
    }

    public override void Exit()
    {
    }
}
