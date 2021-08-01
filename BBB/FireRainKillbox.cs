using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRainKillbox : MonoBehaviour
{
    List<GameObject> blocks;
    List<GameObject> mPlatforms;

    private void Awake()
    {
        blocks = new List<GameObject>();
        mPlatforms = new List<GameObject>();
    }



    public void DestroyBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] != null)
            {
                Destroy(blocks[i]);
            }
        }

        blocks = new List<GameObject>();
    }
    public void MakeMovePlatforms()
    {
        for (int i = 0; i < mPlatforms.Count; i++)
        {
            if (mPlatforms[i] != null)
            {
                mPlatforms[i].GetComponent<PCollision>().setTriggerPhase();
            }
        }

        mPlatforms = new List<GameObject>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DestructablePlatform>() != null)
        {
            blocks.Add(other.gameObject);
        }
        if (other.GetComponent<MP>() != null)
        {
            mPlatforms.Add(other.GetComponentInChildren<PCollision>().gameObject);
        }

    }

}
