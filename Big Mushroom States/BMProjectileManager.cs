using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMProjectileManager : MonoBehaviour
{
    [SerializeField] GameObject mushroomPrefab;
    float speed;
    float timer;
    bool instantiatedNextMushroom;
    bool spawn;
    bool hitPlayer;
    Vector3 direction;

    List<BMProjectilePart> inactiveMushrooms;
    List<BMProjectilePart> activeMushrooms;

    Vector3 nextPos;

    //List<BMProjectilePart> parts;

    public bool FinishedAttack { get;  set; }

    private void Awake()
    {
        speed = 0.1f;
        timer = speed;
        inactiveMushrooms = new List<BMProjectilePart>();
        activeMushrooms = new List<BMProjectilePart>();

    }





    private void OnEnable()
    {
        spawn = true;
        FinishedAttack = false;
        Vector3 playerPos = new Vector3(PlayerMovement.instance.transform.position.x, 0, PlayerMovement.instance.transform.position.z);
        Vector3 pos = new Vector3(transform.position.x, 0, transform.position.z);
        direction = playerPos - pos;
        direction = direction.normalized;
        direction.y = 0;

        //transform.position = PlayerMovement.instance.transform.position - (direction * 7);
        transform.position = pos;
        nextPos = transform.position;
        GetNextPosition();
        hitPlayer = false;


    }


    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (spawn)
            {
                SpawnMushroom();
                timer = speed;
            }



        }
    }

    public void HitPlayer()
    {
        if (!hitPlayer)
        {
            PlayerMovement.instance.transform.GetComponent<hsPlayer>().TakeDamageNoKnockback(1, 0);
            spawn = false;
            StartCoroutine(Deactivate());

            hitPlayer = true;
        }
        
    }

    private void GetNextPosition()
    {
        nextPos += direction;

        float y = -0.5f;
        bool current = false;
        bool previous = false;

        while (!(current == false && previous == true) && y <= 10)
        {
            previous = current;

            if (Physics.CheckSphere(new Vector3(nextPos.x, y, nextPos.z), 0.3f, 1 << 8))
            {
                current = true;
            }
            else
            {
                current = false;
            }
            y += 1;
        }

        y -= 2;
        nextPos = new Vector3(nextPos.x, y, nextPos.z);

        if (y >= 7)
        {
            spawn = false;
            StartCoroutine(Deactivate());
        }
    }


    private void SpawnMushroom()
    {
        if (inactiveMushrooms.Count > 0)
        {
            activeMushrooms.Add(inactiveMushrooms[0]);
            inactiveMushrooms.RemoveAt(0);
            activeMushrooms[activeMushrooms.Count - 1].transform.position = nextPos;
        }
        else
        {
            activeMushrooms.Add(Instantiate(mushroomPrefab, nextPos, Quaternion.identity, transform).GetComponent<BMProjectilePart>());
        }

        activeMushrooms[activeMushrooms.Count - 1].Spawn();
        GetNextPosition();
    }

    public void DeactivateMushroom(BMProjectilePart mushroom)
    {
        if (activeMushrooms.Contains(mushroom))
        {
            inactiveMushrooms.Add(mushroom);
            activeMushrooms.Remove(mushroom);
        }
    }

    public IEnumerator Deactivate()
    {
        //yield return new WaitForSeconds(1.2f);
        //FinishedAttack = true;
        while (activeMushrooms.Count > 0)
        {
            yield return null;
        }

        FinishedAttack = true;
        gameObject.SetActive(false);

    }



}
