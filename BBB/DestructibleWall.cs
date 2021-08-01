using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{

    public static DestructibleWall instance = null;

    [SerializeField] ParticleSystem[] particles;

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

    public void DestroyWall(Vector3 position)
    {
        transform.position = position;

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }


}
