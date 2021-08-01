using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBall : MonoBehaviour
{
    Rigidbody rb;
    float spawnTimer = 2f;
    [SerializeField] int power;
    [SerializeField] Transform sprite;
    Vector3 origen;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origen = transform.position;
        rb.AddForce(Vector3.up * 1000);
        power = Random.Range(20000, 23000);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.up * power);
            spawnTimer = 2;

        }
        if (transform.position.y < origen.y - 1)
        {
            transform.position = origen;
            rb.isKinematic = true;
        }

        if (rb.velocity.y > 0)
        {
            RotateToPlayer(180);
        }
        else
        {
            RotateToPlayer(0);
        }

    }

    private void RotateToPlayer(int z)
    {
        var lookPos = PlayerMovement.instance.transform.position - transform.GetChild(0).position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.GetChild(0).rotation = Quaternion.Slerp(transform.GetChild(0).rotation, rotation, Time.deltaTime * 50);
        transform.GetChild(0).eulerAngles = new Vector3(transform.GetChild(0).eulerAngles.x, transform.GetChild(0).eulerAngles.y, z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            PlayerMovement.instance.GetComponent<hsPlayer>().TakeDamage(1, this.transform.position, 1);
        }

    }


}
