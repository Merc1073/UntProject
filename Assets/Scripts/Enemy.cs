using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject player;
    public GameObject coin;

    public GameScript gamescript;

    public HealthBar healthbar;

    public AudioSource src;
    public AudioClip explosionSound;

    //public ParticleSystem particles;
    //public MeshRenderer mesh;

    public Transform canvasTransform;

    public float forceMultiplier;

    public float coinCounter;

    Vector3 coinPosition;

    Rigidbody rb;

    public int maxHealth;
    public int health;

    public bool particOnce = true;




    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        gamescript = FindObjectOfType<GameScript>();

        src = FindObjectOfType<AudioSource>();

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

            src.pitch = 1;
            //src.clip = explosionSound;
            src.volume = 0.4f;
            src.PlayOneShot(explosionSound);

            Destroy(this.gameObject);


        }

    }

    private void LateUpdate()
    {
        canvasTransform.position = transform.position + new Vector3(0, 4, 2);
        canvasTransform.rotation = Quaternion.Euler(0.25f, 0, 0);
        //canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {

        Vector3 directionToPlayer = transform.position - player.transform.position;
        directionToPlayer = directionToPlayer.normalized * forceMultiplier;

        if (other.gameObject.tag == "Bullet")
        {

            //var em = particles.emission;
            //var dur = particles.duration;

            //em.enabled = true;
            //particles.Play();

            //particOnce = false;

            health -= 1;
            healthbar.SetHealth(health);

            rb.AddForce(directionToPlayer * Time.deltaTime, ForceMode.Impulse);
        }
    }

}
