using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public GameObject player;
    Rigidbody rb;

    public AudioSource src;
    public AudioClip coinSound;

    public float forceMultiplier;
    public float explosionForce;
    public float speed;

    bool distanceTriggered = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        rb.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

        src = FindObjectOfType<AudioSource>();

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
            speed += 0.25f;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            src.pitch = Random.Range(0.5f, 0.8f);
            //src.clip = coinSound;
            src.PlayOneShot(coinSound);

            Destroy(this.gameObject);
        }
    }

}
