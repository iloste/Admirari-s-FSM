using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRainManager : MonoBehaviour
{
    public static FireRainManager instance = null;

    [SerializeField] Transform[] phaseLocations;
    [SerializeField] GameObject prefab;
    [SerializeField] FireRainParticle fireParticleEffect;

    FireRainKillbox killbox;
    bool killboxCalled;
    List<Vector3> spawnList;
    List<int> listCounter;
    float spawnTimer;

    bool spawn;

    List<GameObject> activeFire;
    List<GameObject> inactiveFire;

    List<FireRainParticle> activeFireParticleEffect;
    List<FireRainParticle> inactiveFireParticleEffect;

    public bool Spawn { get { return spawn; } }
    public int Phase { get; set; }

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


    // Start is called before the first frame update
    void Start()
    {
        activeFire = new List<GameObject>();
        inactiveFire = new List<GameObject>();
        activeFireParticleEffect = new List<FireRainParticle>();
        inactiveFireParticleEffect = new List<FireRainParticle>();
        ResetSpawners(Phase);
        killbox = GetComponentInChildren<FireRainKillbox>();
    }

    // Update is called once per frame
    void Update()
    {

        if (spawn)
        {
            if (spawnTimer >= 0)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                spawnTimer = 0.04f;
                int index = Random.Range(0, spawnList.Count);
                GetFire().transform.position = spawnList[index];
                //Instantiate(prefab, spawnList[index], Quaternion.identity);
                listCounter[index]--;

                if (listCounter[index] <= 0)
                {
                    spawnList.RemoveAt(index);
                    listCounter.RemoveAt(index);
                }

                if (spawnList.Count < 10 && killboxCalled == false)
                {
                    killbox.DestroyBlocks();
                    killbox.MakeMovePlatforms();
                    killboxCalled = true;
                }

                if (spawnList.Count == 0)
                {
                    spawn = false;
                }
            }
        }
    }


    public void RainFire()
    {
        spawn = true;
    }

    public void ResetSpawners(int phase)
    {
        transform.position = phaseLocations[phase].position;

        spawnList = new List<Vector3>();

        for (int i = 0; i < 10; i++)
        {
            for (int ii = 0; ii < 5; ii++)
            {
                spawnList.Add(new Vector3(transform.position.x + i * 2, 15, transform.position.z + ii * 2));

            }
        }

        listCounter = new List<int>();

        for (int i = 0; i < spawnList.Count; i++)
        {
            listCounter.Add(3);
        }

        killboxCalled = false;
    }



    private GameObject GetFire()
    {
        if (inactiveFire.Count > 0)
        {
            activeFire.Add(inactiveFire[0]);
            inactiveFire.RemoveAt(0);
            activeFire[activeFire.Count - 1].SetActive(true);
            return activeFire[activeFire.Count - 1];
        }
        else
        {
            activeFire.Add(Instantiate(prefab, transform));
            return activeFire[activeFire.Count - 1];
        }
    }

    public void GetFirePartilceEffect(Vector3 position)
    {
        if (inactiveFireParticleEffect.Count > 0)
        {
            activeFireParticleEffect.Add(inactiveFireParticleEffect[0]);
            inactiveFireParticleEffect.RemoveAt(0);
            activeFireParticleEffect[activeFireParticleEffect.Count - 1].Play(position);
        }
        else
        {
            FireRainParticle x = Instantiate(fireParticleEffect, transform).GetComponent<FireRainParticle>();
            activeFireParticleEffect.Add(x);
            activeFireParticleEffect[activeFireParticleEffect.Count - 1].Play(position);
        }
    }

    public void SetInactiveParticle(FireRainParticle particle)
    {
        activeFireParticleEffect.Remove(particle);
        inactiveFireParticleEffect.Add(particle);
    }

    public void SetInactive(GameObject fire)
    {
        activeFire.Remove(fire);
        inactiveFire.Add(fire);
        fire.SetActive(false);
    }

}

