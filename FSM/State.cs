using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public enum StatesEnum
{
    Default,
    BigOozeMovement,
    BigOozeIdle,
    BigOozeKnockback,
    BigOozeDeath,
    BigOozeSensor,
    LittleDevilDance,
    LittleDevilDance2,
    LittleDevilAttack,
    LittleDevilKnockback,
    LittleDevilDeath,
    LittleDevilIdle,
    LittleDevilPatrol,
    LittleDevilSensor,
    SkullBearRush,
    SkullBearEarthquake,
    SkullBearIdle,
    SkullBearDeath,
    SkullBearRest,
    SkullBearSensor,
    SkullBearKnockback,
    SkullBearStartState,
    BigOozeAttack,
    BigOozeChase,
    SkullBearPunch,
    SkullBearStandState,
    SMPatrol,
    SMAttack,
    SMDeath,
    SMHurt,
    SMIdle,
    SMEscape,
    SMSensor,
    SMGetPlayerInSight,
    BMSensor,
    BMIdle,
    BMSpawnSM,
    BMDeath,
    BMKnockback,
    BMWalk,
    BMAttack,
    BMStageTransition,
    BMBite,
    BMHurt,
    BBBDeath,
    BBBDefaultState,
    BBBFireBall,
    BBBIdle,
    BBBKnockback,
    BBBMoving,
    BBBScythe,
    BBBFireRain,
    BBBSensor,
    BBBStateTransition,
}


public abstract class State
{
    public StatesEnum StateName { get; protected set; }

    public FSM AgentFSM { get; set; }
    public NavMeshAgent NavAgent { get; set; }



    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();
}
