using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRain : MonoBehaviour
{
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        FireRainManager.instance.GetFirePartilceEffect(transform.position);

        CameraShake.instance.IncreaseTrauma(1.1f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<DestructablePlatform>() != null)
            {
                colliders[i].gameObject.SetActive(false);
            }
            else if (colliders[i].tag == "Player")
            {
                PlayerMovement.instance.GetComponent<hsPlayer>().takeDamage(1, gameObject);
            }
        }

        FireRainManager.instance.SetInactive(this.gameObject);
    }
}
