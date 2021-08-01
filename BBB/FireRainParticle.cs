using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRainParticle : MonoBehaviour
{
    ParticleSystem particles;
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    public void Play(Vector3 position)
    {
        particles = GetComponent<ParticleSystem>();

        transform.position = position;
        particles.Play();
        StartCoroutine(timer());
    }


    IEnumerator timer()
    {
        yield return new WaitForSeconds(1f);
        FireRainManager.instance.SetInactiveParticle(this);

    }
}
