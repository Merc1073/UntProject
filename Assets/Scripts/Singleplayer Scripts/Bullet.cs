using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public GameObject particles;
    public MeshRenderer mesh;

    GameScript gameScript;
    GenericPlaySound soundPlay;

    private TrailRenderer trailRenderer;

    public float bulletSpeed;
    public float bulletDuration;
    float timer;

    float appearingSpeed;
    float speed;
    public float magnetSpeed;

    public float magnetDistance;

    public float shrinkSpeed;

    public bool particOnce = true;
    public bool sizeIsMax = false;
    public bool distanceTriggered = false;

    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;

    public Vector3 targetGrowthScale;


    private void Start()
    {
        gameScript = FindObjectOfType<GameScript>();

        soundPlay = GetComponentInParent<GenericPlaySound>();

        trailRenderer = GetComponent<TrailRenderer>();


        StartCoroutine(Grow());
    }

    void Update()
    {


        timer += Time.deltaTime;


        AdjustTrailWidth();


        if(timer >= bulletDuration - 1f)
        {
            StartCoroutine(ShrinkObject());
        }

        if (timer >= bulletDuration)
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
                    speed += magnetSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetEnemy, speed);
                }
            }
        }

        else
        {
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }

        
    }

    void AdjustTrailWidth()
    {
        float averageScale = (transform.localScale.x + transform.localScale.y + transform.localScale.z) / 3f;

        trailRenderer.widthMultiplier = averageScale;
    }

    private IEnumerator Grow()
    {

        Vector3 originalSize = Vector3.zero;

        for (float t = 0; t < growthDuration; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetGrowthScale, t / growthDuration);
            yield return null;
        }

        transform.localScale = targetGrowthScale;

    }

    IEnumerator ShrinkObject()
    {
        while (transform.localScale.x > 0 && transform.localScale.y > 0 && transform.localScale.z > 0)
        {
            Vector3 newScale = transform.localScale - new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed) * Time.deltaTime;

            newScale = Vector3.Max(newScale, Vector3.zero);

            transform.localScale = newScale;

            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {

            //var em = particles.emission;

            //em.enabled = true;

            //transform.parent.position = transform.position;

            //soundPlay.canPlaySound = true;

            //particles.Play();

            //particOnce = false;

            //Destroy(mesh);
            //Invoke(nameof(DestroyObj), 0);

            Instantiate(particles, transform.position, Quaternion.identity);

            Destroy(gameObject);

        }

        if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Ground")
        {
            //var em = particles.emission;

            //em.enabled = true;

            //transform.parent.position = transform.position;

            //particles.Play();

            //particOnce = false;

            //Destroy(mesh);
            //Invoke(nameof(DestroyObj), 0);

            Instantiate(particles, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

    }

}
