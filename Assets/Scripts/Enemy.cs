using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject player;

    public float forceMultiplier;

    Rigidbody rb;

    public float health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Debug.Log(health);

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        Vector3 directionToPlayer = transform.position - player.transform.position;
        directionToPlayer = directionToPlayer.normalized * forceMultiplier;

        
        rb.AddForce(-directionToPlayer * Time.deltaTime);

        if(health <= 0)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            health -= 1;
        }
    }

}
