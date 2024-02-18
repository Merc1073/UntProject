using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiEnemy : NetworkBehaviour
{
    //GameObject player;
    Rigidbody rb;

    public GameObject playerDetection;

    public GameObject particles;

    public GameObject coinOrb;

    MultiGameScript multiGameScript;

    private HealthBar enemyHealthBar;

    //private EnemyPlaySound soundPlay;

    //public ParticleSystem particles;
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

        //player = GameObject.FindGameObjectWithTag("Player");

        multiGameScript = FindObjectOfType<MultiGameScript>();

        //soundPlay = GetComponentInParent<EnemyPlaySound>();

        enemyHealthBar = GetComponentInChildren<HealthBar>();

        currentHealth = maxHealth;

        if (enemyHealthBar)
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

        for (float t = 0; t < growthDuration; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(originalSize, targetGrowthScale, t / growthDuration);
            yield return null;
        }

        transform.localScale = targetGrowthScale;

    }

    void Update()
    {

        if (playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer != null)
        {

            totalForceMultiplier = forceMultiplier + multiGameScript.globalEnemyForceMultiplier;

            Vector3 playerObject = playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.transform.position;

            float distanceToPlayer = Vector3.Distance(transform.position, playerObject);

            Vector3 directionToPlayer = transform.position - playerObject;
            directionToPlayer = directionToPlayer.normalized * (forceMultiplier + multiGameScript.globalEnemyForceMultiplier);

            rb.AddForce(-directionToPlayer * Time.deltaTime);


            if (currentHealth <= 0)
            {

                currentCoinCounter = Random.Range(minCoins, maxCoins);

                while (currentCoinCounter != 0)
                {

                    SpawnCoinServerRpc();

                    currentCoinCounter--;
                }


                multiGameScript.ReduceEnemy();

                CreateParticlesServerRpc();

                DespawnEnemyServerRpc();

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

        if (other.GetComponent<MultiBullet>())
        {
            currentHealth -= 1;

            if (enemyHealthBar)
            {
                enemyHealthBar.UpdateHealthBar(maxHealth, currentHealth);
            }

        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnCoinServerRpc()
    {
        GameObject coin = Instantiate(coinOrb, transform.position + coinPosition, Quaternion.Euler(0, Random.Range(0, 360), 0));
        coin.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateParticlesServerRpc()
    {
        GameObject enemyParticle = Instantiate(particles, transform.position, Quaternion.identity);
        enemyParticle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DespawnEnemyServerRpc()
    {
        GetComponentInParent<NetworkObject>().Despawn();

        Destroy(mesh);
        Destroy(gameObject);
    }

}
