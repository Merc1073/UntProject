using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public GameObject player;
    Rigidbody rb;

    public float forceMultiplier;
    public float explosionForce;

    bool distanceTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        rb.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        Vector3 directionToPlayer = transform.position - player.transform.position;
        //directionToPlayer = directionToPlayer.normalized * forceMultiplier;

        if(distanceToPlayer <= 10f)
        {
            distanceTriggered = true;
        }

        if(distanceTriggered == true)
        {
            rb.AddForce(-directionToPlayer * forceMultiplier, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

}
