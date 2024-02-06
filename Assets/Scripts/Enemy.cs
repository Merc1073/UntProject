using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    public GameObject coin;

    public float forceMultiplier;
    public float explosionForce;
    public float fieldOfImpact;

    public float counter;

    Vector3 coinPosition;

    Rigidbody rb;

    public float health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        Vector3 directionToPlayer = transform.position - player.transform.position;
        directionToPlayer = directionToPlayer.normalized * forceMultiplier;

        
        rb.AddForce(-directionToPlayer * Time.deltaTime);

        if(health <= 0)
        {

            GameObject clone;
            

            while(counter != 0)
            {
                coinPosition = new Vector3(Random.Range(0.5f, -0.5f), 0, Random.Range(0.5f, -0.5f));
                clone = Instantiate(coin, transform.position + coinPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
                //rb.AddExplosionForce(explosionForce, transform.position, fieldOfImpact);
                counter--;
            }
            
            Destroy(this.gameObject);


        }

    }

    private void OnTriggerEnter(Collider other)
    {

        Vector3 directionToPlayer = transform.position - player.transform.position;
        directionToPlayer = directionToPlayer.normalized * forceMultiplier;

        if (other.gameObject.tag == "Bullet")
        {
            health -= 1;
            rb.AddForce(directionToPlayer * Time.deltaTime, ForceMode.Impulse);
        }
    }

}
