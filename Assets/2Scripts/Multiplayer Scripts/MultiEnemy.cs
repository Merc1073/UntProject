using Newtonsoft.Json.Bson;
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
    public bool isDead = false;



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

    private void OnDrawGizmos()
    {
        if(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer)
        {
            Vector3 playerObject = playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.transform.position;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerObject);
        }
    }

    void Update()
    {

        if (currentHealth <= 0 && isDead)
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

        if (playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer != null)
        {

            if(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.GetComponent<MultiMainPlayer>().isAlive.Value == false)
            {
                playerDetection.GetComponent<MultiPlayerDetection>().player.Remove(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer);
                playerDetection.GetComponent<MultiPlayerDetection>().playerObject.Remove(playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.gameObject);
                Debug.Log("player removed");
            }

            totalForceMultiplier = forceMultiplier + multiGameScript.globalEnemyForceMultiplier;

            Vector3 playerObject = playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.transform.position;

            float distanceToPlayer = Vector3.Distance(transform.position, playerObject);

            Vector3 directionToPlayer = transform.position - playerObject;
            directionToPlayer = directionToPlayer.normalized * (forceMultiplier + multiGameScript.globalEnemyForceMultiplier);

            rb.AddForce(-directionToPlayer * Time.deltaTime);
            
 
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

            //if(other.GetComponent<MultiBullet>().GetComponent<NetworkObject>().OwnerClientId == 
            //    playerDetection.GetComponent<MultiPlayerDetection>().targetPlayer.GetComponent<MultiMainPlayer>().GetComponentInParent<NetworkObject>().OwnerClientId)
            //{
            //    UpdatePlayerScoreServerRpc();
            //}

            if(currentHealth <= 0f)
            {
                foreach(GameObject player in playerDetection.GetComponent<MultiPlayerDetection>().playerObject)
                {
                    if(other.GetComponent<MultiBullet>().GetComponent<NetworkObject>().OwnerClientId == 
                        player.GetComponent<MultiMainPlayer>().GetComponentInParent<NetworkObject>().OwnerClientId)
                    {
                        player.GetComponent<MultiMainPlayer>().UpdateEnemyScoreServerRpc(50f);
                        player.GetComponent<MultiMainPlayer>().UpdateScoreServerRpc();
                        isDead = true;
                        Debug.Log("player found, score added.");
                        break;
                    }
                }
            }

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

        multiGameScript.GetComponent<MultiEnemyCount>().allEnemies.Remove(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerScoreServerRpc()
    {
        playerDetection.GetComponent<MultiPlayerDetection>().targetPlayerObject.GetComponent<MultiMainPlayer>().enemyScore.Value += 50f;
    }

}
