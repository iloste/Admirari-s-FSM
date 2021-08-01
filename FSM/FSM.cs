using System.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM : MonoBehaviour
{
    [SerializeField] Enemies enemy;
    [SerializeField] StatesEnum[] desiredStates;
    [Space(10)]
    [SerializeField] StatesEnum initialState;
    State[] states;

    [Space(10)]
    // this is only serialized for testing
    [SerializeField] MonsterStats stats;
    [SerializeField] Animator animator;

    [SerializeField] ParticleSystem[] agentParticles;
    [SerializeField] GameObject dropObject;
    [SerializeField] EnemyDrop drops;
    [SerializeField] BossDrop BossDrop;

    // special states
    State sensorState;
    State deathState;
    State knockbackState;
    State defaultState;
    State previousState;

    public Vector3 StartPos { get; set; }
    public MonsterStats Stats { get { return stats; } set { stats = value; } }
    public State CurrentState { get; private set; }
    public Animator Animator { get { return animator; } private set { animator = value; } }
    public EnemySpawner Spawner { get; private set; }
    public NavMeshAgent NavAgent { get; private set; }
    public bool FacePlayerBool { get; set; }
    public ParticleSystem[] SpawnerParticles { get; set; }
    public ParticleSystem[] AgentParticles { get { return agentParticles; } set { agentParticles = value; } }
    public Enemies Enemy { get { return enemy; } }

    private void Awake()
    {
        dropObject = GameObject.Find("Dropmanager");
        drops = dropObject.GetComponent<EnemyDrop>();
        BossDrop = dropObject.GetComponent<BossDrop>();
        NavAgent = this.GetComponent<NavMeshAgent>();
        SetUpStates();

        StartPos = NavAgent.transform.position;
        Spawner = transform.parent.GetComponent<EnemySpawner>();
    }

    private void OnEnable()
    {
        FacePlayerBool = true;
    }

    void Update()
    {
        sensorState.Execute();
        CurrentState.Execute();
        FacePlayer();
    }


    /// <summary>
    /// Initialises the desired states in the states array
    /// </summary>
    private void SetUpStates()
    {
        states = new State[desiredStates.Length];

        for (int i = 0; i < desiredStates.Length; i++)
        {
            //knockback, death, sensor, and default state should not be initialised here.
            switch (desiredStates[i])
            {
                #region Big Ooze
                case StatesEnum.BigOozeMovement:
                    states[i] = new BigOozeMovementState();
                    break;
                case StatesEnum.BigOozeIdle:
                    states[i] = new BigOozeIdleState();
                    break;
                case StatesEnum.BigOozeAttack:
                    states[i] = new BigOozeAttackState();
                    break;
                case StatesEnum.BigOozeChase:
                    states[i] = new BigOozeChaseState();
                    break;

                #endregion
                #region Little Devil
                case StatesEnum.LittleDevilDance:
                    states[i] = new LittleDevilDanceState();
                    break;
                case StatesEnum.LittleDevilDance2:
                    states[i] = new LittleDevilDance2();
                    break;
                case StatesEnum.LittleDevilAttack:
                    states[i] = new LittleDevilAttackState();
                    break;
                case StatesEnum.LittleDevilIdle:
                    states[i] = new LittleDevilIdleState();
                    break;
                case StatesEnum.LittleDevilPatrol:
                    states[i] = new LittleDevilPatrolState();
                    break;
                #endregion
                #region Skull Bear
                case StatesEnum.SkullBearRush:
                    states[i] = new SkullBearRushState();
                    break;
                case StatesEnum.SkullBearIdle:
                    states[i] = new SkullBearIdleState();
                    break;
                case StatesEnum.SkullBearEarthquake:
                    states[i] = new SkullBearEarthquakeState();
                    break;
                case StatesEnum.SkullBearStartState:
                    states[i] = new SkullBearStartState();
                    break;
                case StatesEnum.SkullBearPunch:
                    states[i] = new SkullBearPunch();
                    break;
                case StatesEnum.SkullBearStandState:
                    states[i] = new SkullBearStandState();
                    break;
                #endregion
                #region Small Mushroom
                case StatesEnum.SMIdle:
                    states[i] = new SMIdleState();
                    break;
                case StatesEnum.SMPatrol:
                    states[i] = new SMPatrolState();
                    break;
                case StatesEnum.SMAttack:
                    states[i] = new SMAttackState();
                    break;
                case StatesEnum.SMEscape:
                    states[i] = new SMEscapeState();
                    break;
                case StatesEnum.SMGetPlayerInSight:
                    states[i] = new SMGetPlayerInSightState();
                    break;
                #endregion
                #region Big Mushroom
                case StatesEnum.BMIdle:
                    states[i] = new BMIdleState();
                    break;
                case StatesEnum.BMSpawnSM:
                    states[i] = new BMSpawnSMState();
                    break;
                case StatesEnum.BMWalk:
                    states[i] = new BMWalkState();
                    break;
                case StatesEnum.BMAttack:
                    states[i] = new BMAttackState();
                    break;
                case StatesEnum.BMStageTransition:
                    states[i] = new BMStageTransition();
                    break;
                case StatesEnum.BMBite:
                    states[i] = new BMBite();
                    break;
                #endregion
                #region Big Bad Boss
                case StatesEnum.BBBFireBall:
                    states[i] = new BBBFireBallState();
                    break;
                case StatesEnum.BBBFireRain:
                    states[i] = new BBBFireRainState();
                    break;
                case StatesEnum.BBBIdle:
                    states[i] = new BBBIdleState();
                    break;
                case StatesEnum.BBBMoving:
                    states[i] = new BBBMovingState();
                    break;
                case StatesEnum.BBBScythe:
                    states[i] = new BBBScytheAttackState();
                    break;
                case StatesEnum.BBBStateTransition:
                    states[i] = new BBBStageTransition();
                    break;
                #endregion
                default:
                    break;
            }

            states[i].AgentFSM = this;
            states[i].NavAgent = this.NavAgent;
        }

        SetUpSpecialStates();
    }

    /// <summary>
    /// sets up the knockback, death, sensor, and default states.
    /// </summary>
    public void SetUpSpecialStates()
    {
        switch (enemy)
        {
            case Enemies.LavaOoze:
            case Enemies.MushroomOoze:
            case Enemies.BigOoze:
                sensorState = new BigOozeSensorState();
                deathState = new BigOozeDeathState();
                knockbackState = new BigOozeKnockbackState();
                break;
            case Enemies.MushroomDevil:
            case Enemies.LittleDevil:
                sensorState = new LittleDevilSensorState();
                deathState = new LittleDevilDeathState();
                knockbackState = new LittleDevilKnockbackState();
                break;
            case Enemies.SkullBear:
                sensorState = new SkullBearSensoryState();
                deathState = new SkullBearDeathState();
                knockbackState = new SkullBearKnockbackState();
                break;
            case Enemies.SmallMushroom:
                sensorState = new SMSensorState();
                deathState = new SMDeathState();
                knockbackState = new SMKnocbackState();
                break;
            case Enemies.BigMushroom:
                sensorState = new BMSensorState();
                deathState = new BMDeathState();
                knockbackState = new BMKnockbackState();
                break;
            case Enemies.BigBadBoss:
                sensorState = new BBBSensorState();
                deathState = new BBBDeathState();
                knockbackState = new BBBKnockbackState();
                break;
        }

        defaultState = new DefaultState();

        sensorState.AgentFSM = this;
        deathState.AgentFSM = this;
        knockbackState.AgentFSM = this;
        defaultState.AgentFSM = this;

        sensorState.NavAgent = this.NavAgent;
        deathState.NavAgent = this.NavAgent;
        knockbackState.NavAgent = this.NavAgent;
        defaultState.NavAgent = this.NavAgent;
    }


    public void EnterStartState()
    {
        CurrentState = GetState(initialState);
        CurrentState.Enter();
        sensorState.Enter();
    }


    /// <summary>
    /// Exits the previous state and Enters the new one
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(StatesEnum newState)
    {
        if (newState != CurrentState.StateName)
        {
            if (CheckStateExists(newState))
            {
                CurrentState.Exit();
                CurrentState = GetState(newState);
                CurrentState.Enter();
            }
            else
            {
                CurrentState = defaultState;
            }
        }
    }

    public void Knockback()
    {
        previousState = CurrentState;
        CurrentState.Exit();
        CurrentState = knockbackState;
        CurrentState.Enter();
    }

    public void Death()
    {
        drops.DropFunc(NavAgent);
        CurrentState.Exit();
        CurrentState = deathState;
        CurrentState.Enter();
    }


    public State GetState(StatesEnum desiredState)
    {
        if (states == null)
        {
            SetUpStates();
        }
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].StateName == desiredState)
            {
                return states[i];
            }
        }

        return defaultState;
    }

    public bool CheckStateExists(StatesEnum desiredState)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].StateName == desiredState)
            {
                return true;
            }
        }

        return false;
    }


    private void FacePlayer()
    {
        if (enemy == Enemies.SkullBear)
        {
            if (PlayerMovement.instance.transform.position.z - transform.position.z < 1.5f && PlayerMovement.instance.transform.position.z - transform.position.z > -1.5f)
            {
                // if player infront, face forwards
                if (transform.position.x < PlayerMovement.instance.transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0, 90, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, -90, 0);
                }
            }
            else
            {
                var lookPos = PlayerMovement.instance.transform.position - NavAgent.transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                NavAgent.transform.rotation = Quaternion.Slerp(NavAgent.transform.rotation, rotation, Time.deltaTime * 50);
            }
        }
        else
        {
            var lookPos = PlayerMovement.instance.transform.position - NavAgent.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            NavAgent.transform.rotation = Quaternion.Slerp(NavAgent.transform.rotation, rotation, Time.deltaTime * 50);
        }

    }


    public void RevertState()
    {
        CurrentState.Exit();
        CurrentState = previousState;
        CurrentState.Enter();
    }
}
