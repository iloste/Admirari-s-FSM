using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearRushState : State
{
    bool HasStart = false;
    bool HasGrunted;
    bool setDestination;
    float idleTimer, maxIdleTime;
    bool front = false;
    float shakeTimer;
    int counter;
    float cooldownTimer;
    int rushCount = 0;

    public SkullBearRushState()
    {
        StateName = StatesEnum.SkullBearRush;
    }


    public override void Enter()
    {
        //front = !front;
        if (HasStart == true && HasGrunted == true)
        {
            Object.FindObjectOfType<AudioManager>().Play("Boss_Rush");
            HasGrunted = true;

        }
        NavAgent.SetDestination(GetDestination());
        // walk animation
        AgentFSM.Animator.SetInteger("State", 1);
        shakeTimer = 0.3f;
        AgentFSM.Stats.CanBeDamaged = false;
        counter = Random.Range(1, 4);

        // rushCounter = 1;
        switch (rushCount)
        {
            case 0:
                rushCount = 1;
                counter = 1;
                break;
            case 1:
                rushCount = 2;
                counter = 2;
                break;
            case 2:
                rushCount = 0;
                counter = 3;
                break;
            case 3:
                rushCount = 1;
                counter = 1;
                break;
            default:
                break;
        }

        cooldownTimer = 1f;
    }


    public override void Execute()
    {
        if (!SkullBearRushCollision.instance.Collided)
        {
            if (NavAgent.isStopped)
            {
                NavAgent.isStopped = false;
            }
        }
        else
        {
            if (!NavAgent.isStopped)
            {
                NavAgent.isStopped = true;
            }
        }

        if (ReachedDestination())
        {
            counter--;
            HasStart = true;
            front = !front;
            if (counter <= 0)
            {
                // Object.FindObjectOfType<AudioManager>().Play("Boss_Stop");
                AgentFSM.ChangeState(StatesEnum.SkullBearEarthquake);
            }
            else
            {
                NavAgent.SetDestination(GetDestination());
                if (HasStart == true && HasGrunted == false)
                {
                    Object.FindObjectOfType<AudioManager>().Play("Boss_Rush");
                    HasGrunted = true;
                }
                else
                {
                    HasGrunted = false;
                }
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
        }


        shakeTimer -= Time.deltaTime;
        if (shakeTimer <= 0)
        {
            CameraShake.instance.IncreaseTrauma(1f);
            shakeTimer = 0.3f;
        }
    }


    public override void Exit()
    {

        NavAgent.isStopped = true;
        NavAgent.ResetPath();
        //AgentFSM.FacePlayerBool = true;
    }


    private bool ReachedDestination()
    {
        float dist = NavAgent.remainingDistance;
        if (dist != Mathf.Infinity && NavAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    private Vector3 GetDestination()
    {
        Vector3 destination;
        if (front)
        {
            destination = AgentFSM.StartPos + new Vector3(9f, 0, 0);
        }
        else
        {
            destination = AgentFSM.StartPos - new Vector3(10f, 0, 0);
        }

        return destination;
    }

}
