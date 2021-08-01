using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOozeChaseState : State
{
    bool isPlaying;
    public BigOozeChaseState()
    {
        StateName = StatesEnum.BigOozeChase;
    }
    public override void Enter()
    {
        isPlaying = false;
    }

    public override void Execute()
    {
        if(isPlaying==false)
        {
            Object.FindObjectOfType<AudioManagerMobs>().Play("Ooze_Chase");
            isPlaying = true;
        }
        if (Vector3.Distance(AgentFSM.transform.position, PlayerMovement.instance.transform.position) <= 1.5f)
        {
            Object.FindObjectOfType<AudioManagerMobs>().Stop("Ooze_Chase");
            AgentFSM.ChangeState(StatesEnum.BigOozeAttack);
        }
        else
        {
            NavAgent.SetDestination(PlayerMovement.instance.transform.position);
        }
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }



}
