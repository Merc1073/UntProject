using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    public GameObject coin;

    public GameScript gamescript;

    public HealthBar healthbar;

    public AudioSource audiosource;
    public AudioClip yoda;

    public Transform canvasTransform;

    public float forceMultiplier;

    public float coinCounter;

    Vector3 coinPosition;

    Rigidbody rb;

    public int maxHealth;
    public int health;




    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        gamescript = FindObjectOfType<GameScript>();

        audiosource = FindObjectOfType<AudioSource>();

        health = maxHealth;
        healthbar.SetMaxHealth(maxHealth);

        
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
            

            while(coinCounter != 0)
            {
                coinPosition = new Vector3(Random.Range(0.5f, -0.5f), 0, Random.Range(0.5f, -0.5f));
                clone = Instantiate(coin, transform.position + coinPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
                coinCounter--;
            }

            gamescript.ReduceEnemy();

            audiosource.pitch = 1;
            //audiosource.clip = yoda;
            audiosource.PlayOneShot(yoda);

            Destroy(this.gameObject);


        }

    }

    private void LateUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Vector3 directionToPlayer = transform.position - player.transform.position;
        directionToPlayer = directionToPlayer.normalized * forceMultiplier;

        if (other.gameObject.tag == "Bullet")
        {
            health -= 1;
            healthbar.SetHealth(health);

            rb.AddForce(directionToPlayer * Time.deltaTime, ForceMode.Impulse);
        }
    }

}
