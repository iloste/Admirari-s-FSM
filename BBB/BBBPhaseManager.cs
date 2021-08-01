using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBPhaseManager : MonoBehaviour
{
    public static BBBPhaseManager instance = null;

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
            Destroy(this);
        }

        agentFSM = transform.parent.GetComponent<FSM>();
        Phase = -1;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
