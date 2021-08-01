using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMProjectilePart : MonoBehaviour
{
    Animator animator;
    BMProjectileManager manager;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        manager = GetComponentInParent<BMProjectileManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
       // Spawn();   
    }

    /// <summary>
    /// called by projectile manager
    /// </summary>
    public void Spawn()
    {
        gameObject.SetActive(true);

        StartCoroutine(RecedeMushroom());
    }

    IEnumerator RecedeMushroom()
    {
        // wait for mushroom to grow + some time to stay still
        yield return new WaitForSeconds(1.2f);
        animator.SetTrigger("Trigger");

        // wait for mushroom to go back into the ground
        yield return new WaitForSeconds(0.6f);
        gameObject.SetActive(false);
        manager.DeactivateMushroom(this);
    }
}
