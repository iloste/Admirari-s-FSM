using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMProjectileCollision : MonoBehaviour
{
    BMProjectileManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = transform.parent.parent.GetComponent<BMProjectileManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            manager.HitPlayer();
        }
    }
}
