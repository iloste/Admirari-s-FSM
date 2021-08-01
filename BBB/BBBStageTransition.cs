using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBStageTransition : State
{
    Vector3[] positions;
    bool setUp;
    BBBPhaseTrigger[] triggers;
    int phase = -1;
    bool lerping;
    bool moved;

    public BBBStageTransition()
    {
        StateName = StatesEnum.BBBStateTransition;
    }

    public override void Enter()
    {
        AgentFSM.Animator.SetInteger("State", 5);
        AgentFSM.Animator.SetTrigger("Trigger");

        if (!setUp)
        {
            positions = new Vector3[4];
            positions[0] = new Vector3(89, 1.5f, 8);
            positions[1] = new Vector3(121, 1.5f, 8);
            positions[2] = new Vector3(153, 1.5f, 8);
            positions[3] = new Vector3(159, 1.5f, 8);
            triggers = AgentFSM.transform.parent.GetComponentsInChildren<BBBPhaseTrigger>();
            setUp = true;
        }

        phase++;
        if (phase > 3)
        {
            phase = 3;
        }

        lerping = false;
        moved = false;
    }

    public override void Execute()
    {
        if (!moved)
        {
            if (!lerping)
            {
                MovePosition();
            }
        }
        else
        {
            if (phase == 3)
            {
                AgentFSM.Death();
            }
            else if (triggers[phase].Entered)
            {
                if(phase==0)
                {
                    
                    Object.FindObjectOfType<AudioManager>().Play("fire earthquake");
                    Object.FindObjectOfType<AudioManager>().Play("Boss_DIE");
                    

                }
                if (phase == 1)
                {
                    Object.FindObjectOfType<AudioManager>().Play("fire earthquake");
                    Object.FindObjectOfType<AudioManager>().Play("Boss_Taunt_1");

                }
                if(phase==2)
                {

                    Object.FindObjectOfType<AudioManager>().Play("fire earthquake");
                    Object.FindObjectOfType<AudioManager>().Play("Boss_Taunt_3");
                    
                }
                FireRainManager.instance.ResetSpawners(phase);
                AgentFSM.ChangeState(StatesEnum.BBBFireRain);
            }
        }

    }

    public override void Exit()
    {
    }

    private void MovePosition()
    {
        lerping = true;
        Vector3 origin = AgentFSM.transform.position;
        Vector3 destination = positions[phase];
        //Vector3 destination = positions[1];
        AgentFSM.StartCoroutine(Lerp(origin, destination));


    }

    private void DestroyWall()
    {
        BBBWall[] blocks = triggers[phase].GetComponentsInChildren<BBBWall>();
        //for (int i = 0; i < blocks.Length; i++)
        //{
        //    blocks[i].gameObject.SetActive(false);
        //}
        if (triggers[phase].transform.childCount > 0)
        {
            triggers[phase].transform.GetChild(0).gameObject.SetActive(false) ;
        }
        if (phase > 0)
        {
            DestructibleWall.instance.DestroyWall(triggers[phase].transform.position);
        }
    }

    IEnumerator Lerp(Vector3 origin, Vector3 destination)
    {
        DestroyWall();
        float prc = 0;
        float speed;

        if (phase == 3)
        {
            speed = 3;
        }
        else
        {
            speed = 1;
        }

        while (prc < 1)
        {
            prc += Time.deltaTime * speed;
            NavAgent.transform.position = Vector3.Lerp(origin, destination, prc);
            yield return null;
        }

        AgentFSM.Animator.SetInteger("State", 0);
        AgentFSM.Animator.SetTrigger("Trigger");
        moved = true;
    }
}
