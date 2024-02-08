using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;
    float timer;

    float appearingSpeed;
    public float speed;

    public float magnetDistance;

    public bool particOnce = true;
    public bool sizeIsMax = false;
    public bool distanceTriggered = false;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    GameScript gameScript;

    private void Start()
    {
        transform.localScale = Vector3.zero;

        gameScript = FindObjectOfType<GameScript>();
    }

    void Update()
    {

        timer += Time.deltaTime;

        if (timer >= 2f)
        {
            Destroy(this.gameObject);
        }

        if (sizeIsMax == false)
        {

            appearingSpeed += Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), Time.deltaTime * 3);

            if (appearingSpeed >= 1f)
            {
                sizeIsMax = true;
            }

        }

        if (gameScript.isMagnetPowerUpActive == true)
        {

            transform.position += transform.forward * bulletSpeed * Time.deltaTime;

            if(gameObject.transform.GetChild(0).GetComponent<EnemyDetection>().targetEnemy != null)
            {
                Vector3 targetEnemy = gameObject.transform.GetChild(0).GetComponent<EnemyDetection>().targetEnemy.transform.position;
                float distanceToEnemy = Vector3.Distance(transform.position, targetEnemy);


                if (distanceToEnemy <= magnetDistance)
                {
                    distanceTriggered = true;
                }

                else
                {
                    distanceTriggered = false;
                }

                if (distanceTriggered == true)
                {
                    speed += 0.25f;
                    transform.position = Vector3.MoveTowards(transform.position, targetEnemy, speed * Time.deltaTime);
                }
            }
        }

        else
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }

        
    }


    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground") && particOnce)
        {

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

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}
