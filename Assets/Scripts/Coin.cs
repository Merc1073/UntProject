using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    GameObject playerObject;

    MainPlayer playerScript;
    BulletPoint bulletReticle;

    Rigidbody rb;

    public AudioSource src;
    public AudioClip coinSound;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    public float forceMultiplier;
    public float explosionForce;
    public float speed;

    public float maxDistanceToPlayer;

    bool distanceTriggered = false;
    public bool particOnce = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        playerObject = GameObject.FindGameObjectWithTag("Player");

        playerScript = FindObjectOfType<MainPlayer>();
        bulletReticle = FindObjectOfType<BulletPoint>();

        rb.AddForce(transform.forward * explosionForce, ForceMode.Impulse);

        src = FindObjectOfType<AudioSource>();

    }

    void Update()
    {

        if(transform.position.y < 0.52)
        {
            transform.position += new Vector3(0, 0.1f, 0);
        }

        if(transform.position.y > 0.52)
        {
            transform.position += new Vector3(0, -0.1f, 0);
        }

        if(playerObject != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerObject.transform.position);

            Vector3 directionToPlayer = transform.position - playerObject.transform.position;
            //directionToPlayer = directionToPlayer.normalized * forceMultiplier;

            if (distanceToPlayer <= maxDistanceToPlayer)
            {
                distanceTriggered = true;
            }

            if (distanceTriggered == true)
            {
                speed += 0.25f;
                transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(playerObject != null)
        {
            if (other.gameObject.tag == "Player" && particOnce)
            {
                //src.pitch = Random.Range(0.5f, 0.8f);
                src.volume = 0.2f;
                src.clip = coinSound;
                src.Play();

                var em = particles.emission;
                var dur = particles.main.duration;

                em.enabled = true;

                transform.parent.position = transform.position;

                particles.Play();

                particOnce = false;

                playerScript.AddCoins(1);
                bulletReticle.IncreaseFireRate(0.1f);

                Destroy(mesh);
                Invoke(nameof(DestroyObj), 0);

            }
        }
        
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}
