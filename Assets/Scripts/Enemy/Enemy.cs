using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    GameObject player;
    Rigidbody rb;

    public GameObject coin;

    GameScript gamescript;

    private HealthBar enemyHealthBar;

    private EnemyPlaySound soundPlay;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    public Transform canvasTransform;


    [SerializeField] float growthDuration;
    [SerializeField] float fadeDuration;


    public float forceMultiplier;
    public float totalForceMultiplier;

    public int currentCoinCounter;
    public int minCoins;
    public int maxCoins;

    public int maxHealth;
    public int currentHealth;

    public Vector3 coinPosition;
    public Vector3 enemyBulletPointPosition;
    public Vector3 tranDif;

    public Vector3 originalTransformScale;
    public Vector3 targetGrowthScale;

    public bool particOnce = true;



    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        gamescript = FindObjectOfType<GameScript>();

        soundPlay = GetComponentInParent<EnemyPlaySound>();

        enemyHealthBar = GetComponentInChildren<HealthBar>();

        currentHealth = maxHealth;

        if(enemyHealthBar)
        {
            enemyHealthBar.UpdateHealthBar(maxHealth, currentHealth);
        }

        transform.localScale = Vector3.zero;

        StartCoroutine(Grow());
        StartCoroutine(FadeIn());

    }

    private IEnumerator FadeIn()
    {

        Color color = mesh.material.color;
        float targetAlpha = 1f;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0f, targetAlpha, t / fadeDuration);
            mesh.material.color = color;
            yield return null;

        }

        color.a = targetAlpha;
        mesh.material.color = color;

    }

    private IEnumerator Grow()
    {

        Vector3 originalSize = originalTransformScale;

        for(float t = 0; t < growthDuration; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetGrowthScale, t/ growthDuration);
            yield return null;
        }

        transform.localScale = targetGrowthScale;

    }

    void Update()
    {

        if(player != null)
        {

            totalForceMultiplier = forceMultiplier + gamescript.globalEnemyForceMultiplier;

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            Vector3 directionToPlayer = transform.position - player.transform.position;
            directionToPlayer = directionToPlayer.normalized * (forceMultiplier + gamescript.globalEnemyForceMultiplier);


            rb.AddForce(-directionToPlayer * Time.deltaTime);

            if (currentHealth <= 0)
            {

                GameObject clone;
                currentCoinCounter = Random.Range(minCoins, maxCoins);

                while (currentCoinCounter != 0)
                {

                    //coinPosition = new Vector3(Random.Range(0.5f, -0.5f), -2.0f, Random.Range(0.5f, -0.5f));
                    clone = Instantiate(coin, transform.position + coinPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    currentCoinCounter--;
                }

                gamescript.ReduceEnemy();


                soundPlay.canPlaySound = true;

                var em = particles.emission;

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

                if(enemyHealthBar)
                {
                    enemyHealthBar.UpdateHealthBar(maxHealth, currentHealth);
                }

                rb.AddForce(directionToPlayer * Time.deltaTime, ForceMode.Impulse);
            }
        }
      
    }

    void DestroyObj()
    {
        Destroy(gameObject);
    }

}
