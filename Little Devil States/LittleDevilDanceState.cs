using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilDanceState : State
{

    bool dodging;
    bool setDestination;
    float changeRangeTimer;
    float danceRange;

    public LittleDevilDanceState()
    {
        StateName = StatesEnum.LittleDevilDance;
    }


    public override void Enter()
    {
        changeRangeTimer = 1;
        danceRange = 2;
        dodging = false;
        AgentFSM.Animator.SetInteger("State", 4);
        AgentFSM.Animator.SetTrigger("Trigger");
    }

    public override void Execute()
    {
        // Face player
        //  NavAgent.transform.LookAt(PlayerMovement.instance.transform);
        var lookPos = PlayerMovement.instance.transform.position -NavAgent. transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        NavAgent.transform.rotation = Quaternion.Slerp(NavAgent.transform.rotation, rotation, Time.deltaTime * 50);

        // trigger dodge
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // add delay???????????????????????????????????????????
            // chance of dodging
            int rand = Random.Range(0, 5);
            if (rand <= 3)
            {
                dodging = true;
                
                Object.FindObjectOfType<AudioManagerDevil>().Play("Taunt");
                    
            }
        }


        // if player near
        if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 10)
        {
            Dancing();
        }

        ChangeDanceRange();
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }

    private void Dancing()
    {
        // Get vector between player and Agent
        Vector3 direction = NavAgent.transform.position - PlayerMovement.instance.transform.position;
        direction.Normalize();


        if (dodging)
        {
            // Choose dodge direction
            if (!setDestination)
            {
                int rand = Random.Range(1, 3);
                Vector3 destination = Vector3.zero;

                switch (rand)
                {
                    case 0:
                        // currently not used
                        destination = NavAgent.transform.position + (direction * 1);
                        break;
                    case 1:
                        destination = NavAgent.transform.position + (NavAgent.transform.right * 1);
                        break;
                    case 2:
                        destination = NavAgent.transform.position - (NavAgent.transform.right * 1);
                        break;
                }

                NavAgent.SetDestination(destination);
                setDestination = true;
            }

            // When at destination, go back to dancing
            float dist = NavAgent.remainingDistance;
            if (dist != Mathf.Infinity && NavAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
            {
                // chance of counter attack
                int rand = Random.Range(0, 3);
                if (rand == 0)
                {
                    
                    AgentFSM.ChangeState(StatesEnum.LittleDevilAttack);
                }

                // return to keeping distance
                dodging = false;
                setDestination = false;
            }
        }
        else
        {
            // Stay 2 units away from player
            NavAgent.SetDestination(PlayerMovement.instance.transform.position + (direction * danceRange));
        }

    }

    private void ChangeDanceRange()
    {
        if (changeRangeTimer > 0)
        {
            changeRangeTimer -= Time.deltaTime;
        }
        else
        {
            changeRangeTimer = 1f;
            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                danceRange = 1f;
            }
            else if (rand == 1)
            {
                danceRange = 1.5f;
            }
            else
            {
                danceRange = 2f;
            }
        }
    }
}
