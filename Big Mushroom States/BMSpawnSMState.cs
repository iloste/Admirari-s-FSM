using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMSpawnSMState : State
{
    EnemySpawner[] spawners;
    SpawnerIdentifier[] identifiers;
    int phase;

    public BMSpawnSMState()
    {
        StateName = StatesEnum.BMSpawnSM;
    }

    public override void Enter()
    {
        if (spawners == null)
        {
            spawners = AgentFSM.transform.parent.GetComponentsInChildren<EnemySpawner>();
            identifiers = new SpawnerIdentifier[spawners.Length];

            for (int i = 0; i < spawners.Length; i++)
            {
                identifiers[i] = spawners[i].GetComponent<SpawnerIdentifier>();
            }
        }
    }

    public override void Execute()
    {
        for (int i = 0; i < identifiers.Length; i++)
        {
            if (identifiers[i].Phase == phase)
            {
                spawners[i].SpawnEnemyStart();
            }
        }

        AgentFSM.ChangeState(StatesEnum.BMIdle);
        
    }

    public override void Exit()
    {
        phase++;
    }
}
