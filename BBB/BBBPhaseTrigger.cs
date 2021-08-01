using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBBPhaseTrigger : MonoBehaviour
{
    [SerializeField] int phase;


    public BoxCollider Collider { get; private set; }
    public bool Entered { get; set; }
    public int Phase { get { return phase; } }

    private void Awake()
    {
        Collider = GetComponent<BoxCollider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Entered = true;
        }
    }
}
