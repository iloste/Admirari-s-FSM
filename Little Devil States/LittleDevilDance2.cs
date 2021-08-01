using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevilDance2 : State
{

    bool dodging;
    bool setDestination;
    float changeRangeTimer;
    float danceRange;
    float attackTimer;
    float maxAttackTime;
    public LittleDevilDance2()
    {
        StateName = StatesEnum.LittleDevilDance2;
    }


    public override void Enter()
    {
        changeRangeTimer = 1;
        danceRange = 2;
        dodging = false;
        AgentFSM.Animator.SetInteger("State", 4);
        AgentFSM.Animator.SetTrigger("Trigger");
        ResetAttackTime();

    }

    public override void Execute()
    {
        // If on same plane as player
      //  if (Mathf.Abs(NavAgent.transform.position.y - PlayerMovement.instance.Height) < 0.5f)
        {
            // dodge if player attacks, chance of counter attack
            Dodge();
            // stay in front/behind player on x axis if they're near. 
            Dance();
            // Attack after random time of not dodging.
            Attack();
        }
    }

    public override void Exit()
    {
        NavAgent.isStopped = true;
        NavAgent.ResetPath();
    }

    private void Dodge()
    {
        // if player attacks, dodge + reset attack timer
        if (!dodging && Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftControl))
        {
            dodging = true;
            Object.FindObjectOfType<AudioManagerDevil>().Play("Taunt");
               
           
            ResetAttackTime();
        }

        if (dodging)
        {
            Vector3 direction = NavAgent.transform.position - PlayerMovement.instance.transform.position;
            direction.Normalize();

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
                        // destination = NavAgent.transform.position + (NavAgent.transform.right * 1);
                        destination = NavAgent.transform.position + new Vector3(0, 0, 1);
                        break;
                    case 2:
                        //destination = NavAgent.transform.position - (NavAgent.transform.right * 1);
                        destination = NavAgent.transform.position - new Vector3(0, 0, 1);
                        break;
                }

                NavAgent.SetDestination(destination);
                setDestination = true;
            }

            // When at destination, go back to dancing
            float dist = NavAgent.remainingDistance;
            if (dist != Mathf.Infinity && NavAgent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && NavAgent.remainingDistance == 0)
            {
                ChanceToCounterAttack();

                // return to keeping distance
                dodging = false;
                setDestination = false;
            }
        }
    }



    private void Dance()
    {
        if (!dodging)
        {
            // if player near
            if ((Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 6))
            {
                // Get vector between player and Agent
                Vector3 direction = NavAgent.transform.position - PlayerMovement.instance.transform.position;
                direction.Normalize();

                // Stay 2 units infront/behind the player
                if (PlayerMovement.instance.transform.position.x < NavAgent.transform.position.x)
                {
                    NavAgent.SetDestination(PlayerMovement.instance.transform.position + new Vector3(2, 0, 0));
                }
                else
                {
                    NavAgent.SetDestination(PlayerMovement.instance.transform.position - new Vector3(2, 0, 0));
                }
            }
        }
    }


    /// <summary>
    /// Countdown till attack player
    /// </summary>
    private void Attack()
    {
        if (Vector3.Distance(PlayerMovement.instance.transform.position, NavAgent.transform.position) < 3)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                AgentFSM.ChangeState(StatesEnum.LittleDevilAttack);
            }
        }
    }

   


    private void ResetAttackTime()
    {
        attackTimer = Random.Range(1.5f, 1.5f);
    }



    private void ChanceToCounterAttack()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            AgentFSM.ChangeState(StatesEnum.LittleDevilAttack);
        }
    }


    // not in use
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


    // moved to FSM
    private void FacePlayer()
    {
        var lookPos = PlayerMovement.instance.transform.position - NavAgent.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        NavAgent.transform.rotation = Quaternion.Slerp(NavAgent.transform.rotation, rotation, Time.deltaTime * 50);
    }


}
