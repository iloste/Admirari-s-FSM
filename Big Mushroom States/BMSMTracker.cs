using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMSMTracker : MonoBehaviour
{
    public static BMSMTracker instance = null;
    public int Phase { get; private set; }
    public int MushroomCount { get; private set; }
    FSM agentFSM;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        agentFSM = transform.parent.GetComponent<FSM>();
        Phase = 0;
        MushroomCount = 1;
       
    }
    private void Start()
    {
        agentFSM.Stats.CanBeDamaged = false;
    }


    public void MushroomKilled()
    {
        MushroomCount--;
        if (MushroomCount <= 0)
        {
            agentFSM.Stats.CanBeDamaged = true;
        }
    }

    public void ChangePhase()
    {
        Phase++;
        agentFSM.Stats.CanBeDamaged = false;
        switch (Phase)
        {
            case 0:
                MushroomCount = 1;
                break;
            case 1:
                MushroomCount = 2;
                break;
            case 2:
                MushroomCount = 4;
                break;
            default:
                break;
        }
    }
}
