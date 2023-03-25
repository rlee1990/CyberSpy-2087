using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed, bulletLife;

    public Rigidbody myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBullet();

        bulletLife -= Time.deltaTime;

        if (bulletLife < 0)
        {
            Destroy(gameObject);
        }
    }

    private void MoveBullet()
    {
        myRigidbody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.comparetag("enemy"))
        //{
        //    destroy(other.gameobject);
        //}

        Destroy(gameObject);
    }
}
