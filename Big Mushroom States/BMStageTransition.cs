using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMStageTransition : State
{
    int phase = -1;
    bool setUp;
    Vector3[] positions;
    float growTimer;
    float shrinkTimer;
    bool shrink;
    bool grow;
    bool animSet;
    StageTrigger[] triggers;
    StageTrigger activeTrigger;
 
    public BMStageTransition()
    {
        StateName = StatesEnum.BMStageTransition;
    }



    public override void Enter()
    {
        NavAgent.enabled = false;

        if (!setUp)
        {
            positions = new Vector3[3];
            positions[0] = new Vector3(18, 0, 7);
            positions[1] = new Vector3(48, 0.6f, 8);
            positions[2] = new Vector3(83, 0.6f, 8);
            triggers = AgentFSM.transform.parent.GetComponentsInChildren<StageTrigger>();
            setUp = true;
        }

        phase++;

        if (phase == 0)
        {
            shrink = false;
            grow = true;
        }
        else
        {
            shrink = true;
            grow = false;
        }

        activeTrigger = GetActiveTrigger(phase);
        if (activeTrigger.Collider)
        {
            activeTrigger.Collider.enabled = true;  
        }

        shrinkTimer = 2f;
        growTimer = 2f;
        animSet = false;

        AgentFSM.Animator.SetInteger("State", 8);
        AgentFSM.Animator.SetTrigger("Trigger");
    }

    public override void Execute()
    {
        if (shrink)
        {
            if (animSet == false)
            {
                AgentFSM.Animator.SetInteger("State", 7);
                AgentFSM.Animator.SetTrigger("Trigger");
                animSet = true;
            }

            if (shrinkTimer > 0)
            {
                shrinkTimer -= Time.deltaTime;
            }
            else
            {
                NavAgent.transform.position = positions[phase];
                BMSMTracker.instance.ChangePhase();
                shrink = false;
                grow = true;
                animSet = false;
            }
        }
        else if (grow && triggers[phase].Entered)
        {
            if (animSet == false)
            {
                AgentFSM.Animator.SetInteger("State", 6);
                AgentFSM.Animator.SetTrigger("Trigger");
                animSet = true;
            }

            if (growTimer > 0)
            {
                growTimer -= Time.deltaTime;
            }
            else
            {
                grow = false;
                animSet = false;
                AgentFSM.ChangeState(StatesEnum.BMSpawnSM);
            }
        }
    }

    public override void Exit()
    {
        AgentFSM.GetComponent<BoxCollider>().enabled = true;
        triggers[phase].Entered = false;
        activeTrigger.Collider.enabled = true;

    }

    private StageTrigger GetActiveTrigger(int phase)
    {
        for (int i = 0; i < triggers.Length; i++)
        {
            if (triggers[i].Phase == phase)
            {
                return triggers[i];
            }
        }

        return null;
    }



}
