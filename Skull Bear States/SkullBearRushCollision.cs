using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBearRushCollision : MonoBehaviour
{
    public static SkullBearRushCollision instance;

    public bool Collided { get; private set; }
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (!Collided)
            {
                //Collided = true;
                StartCoroutine(Timer());
            }
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(.5f);
        Collided = false;
    }
}
