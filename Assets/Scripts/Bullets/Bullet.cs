using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float bulletSpeed;
    float timer;

    float appearingSpeed;
    float speed;
    public float magnetSpeed;

    public float magnetDistance;

    public bool particOnce = true;
    public bool sizeIsMax = false;
    public bool distanceTriggered = false;

    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;

    public Vector3 originalTransformScale;
    public Vector3 targetGrowthScale;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    GameScript gameScript;

    private void Start()
    {
        gameScript = FindObjectOfType<GameScript>();

        StartCoroutine(Grow());
    }

    private IEnumerator Grow()
    {

        Vector3 originalSize = originalTransformScale;

        for (float t = 0; t < growthDuration; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetGrowthScale, t / growthDuration);
            yield return null;
        }

        transform.localScale = targetGrowthScale;

    }

    void Update()
    {

        timer += Time.deltaTime;

        if (timer >= 2f)
        {
            Destroy(this.gameObject);
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
                    speed += magnetSpeed;
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
        if((other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground"))
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
