using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    GameObject player;
    public GameObject coin;

    GameScript gamescript;

    private HealthBar enemyHealthBar;

    public AudioSource src;
    public AudioClip explosionSound;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    public Transform canvasTransform;

    public float forceMultiplier;

    float appearingSpeed;

    public int currentCoinCounter;
    public int minCoins;
    public int maxCoins;

    Vector3 coinPosition;
    public Vector3 enemyBulletPointPosition;
    public Vector3 tranDif;

    Rigidbody rb;

    public int maxHealth;
    public int currentHealth;

    public bool particOnce = true;




    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        gamescript = FindObjectOfType<GameScript>();

        src = FindObjectOfType<AudioSource>();

        enemyHealthBar = GetComponentInChildren<HealthBar>();

        currentHealth = maxHealth;
        enemyHealthBar.UpdateHealthBar(maxHealth, currentHealth);

        transform.localScale = Vector3.zero;
    }

    void Update()
    {

        if(appearingSpeed < 3)
        {
            appearingSpeed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), appearingSpeed * Time.deltaTime);
        }


        if(player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            Vector3 directionToPlayer = transform.position - player.transform.position;
            directionToPlayer = directionToPlayer.normalized * forceMultiplier;


            rb.AddForce(-directionToPlayer * Time.deltaTime);

            if (currentHealth <= 0)
            {

                GameObject clone;
                currentCoinCounter = Random.Range(minCoins, maxCoins);

                while (currentCoinCounter != 0)
                {

                    coinPosition = new Vector3(Random.Range(0.5f, -0.5f), 0, Random.Range(0.5f, -0.5f));
                    clone = Instantiate(coin, transform.position + coinPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    currentCoinCounter--;
                }

                gamescript.ReduceEnemy();

                src.pitch = 1;
                //src.clip = explosionSound;
                src.volume = 0.4f;
                src.PlayOneShot(explosionSound);

                var em = particles.emission;
                var dur = particles.main.duration;

                em.enabled = true;

                transform.parent.position = transform.position;

                particles.Play();

                particOnce = false;

                Destroy(mesh);
                Invoke(nameof(DestroyObj), 0);

            }
        }
        

    }

    private void LateUpdate()
    {
        canvasTransform.position = transform.position + new Vector3(0, 4, 2);
        canvasTransform.rotation = Quaternion.Euler(90, 0, 0);
        //canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(player != null)
        {
            Vector3 directionToPlayer = transform.position - player.transform.position;
            directionToPlayer = directionToPlayer.normalized * forceMultiplier;

            if (other.gameObject.tag == "Bullet")
            {
                currentHealth -= 1;
                enemyHealthBar.UpdateHealthBar(maxHealth, currentHealth);

                rb.AddForce(directionToPlayer * Time.deltaTime, ForceMode.Impulse);
            }
        }
        
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}
