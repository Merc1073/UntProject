using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{

    [Header("Game Objects")]
    public GameObject Player;
    public GameObject Enemy;
    public GameObject BulletPoint;
    public GameObject Reticle;

    public GameObject MagnetPowerUp;
    public GameObject TripleBulletPowerUp;

    private MainPlayer playerScript;
    private BulletPoint bulletReticle;

    [Header("Special Booleans")]
    public bool isMagnetPowerUpActive = false;
    public bool isTripleBulletPowerUpActive = false;

    public bool isGameModeRapidFire = false;
    public bool isGameModeGrowing = false;

    [Header("Texts")]
    [SerializeField] Text fireRateText;
    [SerializeField] Text respawnTimerText;
    [SerializeField] Text enemyCountText;
    [SerializeField] Text timerText;


    [Header("Positions")]
    public Vector3 playerSpawn;
    public Vector3 enemySpawn;
    public Vector3 tranDif;

    public Vector3 bulletPointSpawn;
    public Vector3 reticlePointSpawn;

    public Vector3 magnetPowerUpSpawn;
    public Vector3 tripBulletPowerUpSpawn;

    [Header("Number Variables")]
    [SerializeField] float totalTime;
    [SerializeField] int seconds;
    [SerializeField] int respawnSeconds;

    [SerializeField] float originalTimerSpawnMagnetPowerUp;
    [SerializeField] float magnetRespawnTimer;

    [SerializeField] float originalTimerSpawnTripleBulletPowerUp;
    [SerializeField] float tripleBulletRespawnTimer;

    public float enemyRespawnTimer;


    public float newEnemyTimer;

    [Header("Booleans")]
    public bool enemyFull = false;
    public bool keepReducingSpawnTimer = false;

    public bool canSpawnEnemies = false;
    public bool canSpawnMagnetPowerUp = false;
    public bool canSpawnTripleBulletPowerUp = false;


    public int enemyCounter;
    public int maxEnemies;

    public bool spawnMagnetPowerUpNow = false;
    public bool spawnTripleBulletPowerUpNow = false;



    void Start()
    {
        Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));

        //Instantiate(MagnetPowerUp, magnetPowerUpSpawn, Quaternion.Euler(0, 0, 0));
        //Instantiate(TripleBulletPowerUp, tripBulletPowerUpSpawn, Quaternion.Euler(0, 0, 0));


        playerScript = FindObjectOfType<MainPlayer>();
        bulletReticle = FindObjectOfType<BulletPoint>();

    }

    void Update()
    {

        if(spawnMagnetPowerUpNow == true)
        {
            DebugSpawnMagnetPowerUp();
        }

        if(spawnTripleBulletPowerUpNow == true)
        {
            DebugSpawnTripleBulletPowerUp();
        }


        if(canSpawnMagnetPowerUp == true)
        {
            magnetRespawnTimer -= Time.deltaTime;

            if(magnetRespawnTimer <= 0) 
            {
                SpawnMagnetPowerUp();

                magnetRespawnTimer = originalTimerSpawnMagnetPowerUp;
            }
        }

        if(canSpawnTripleBulletPowerUp == true)
        {
            tripleBulletRespawnTimer -= Time.deltaTime;

            if (tripleBulletRespawnTimer <= 0)
            {
                SpawnTripleBulletPowerUp();

                tripleBulletRespawnTimer = originalTimerSpawnTripleBulletPowerUp; 
            }
        }

        if (playerScript != null)
        {

            totalTime += Time.deltaTime;
            seconds = (int)(totalTime);

            fireRateText.text = "Fire Rate: " + bulletReticle.roundsPerSecond.ToString();
            respawnTimerText.text = "Enemy Respawn in: " + respawnSeconds.ToString();
            enemyCountText.text = "Total Enemies in arena: " + enemyCounter.ToString();
            timerText.text = "Timer: " + seconds.ToString();

            if (canSpawnEnemies == true)
            {

                if (enemyCounter >= maxEnemies)
                {
                    enemyFull = true;
                }

                if (enemyCounter < maxEnemies)
                {
                    enemyFull = false;
                }

                if (enemyFull == false)
                {
                    if(keepReducingSpawnTimer == true)
                    {
                        newEnemyTimer -= 0.0001f;
                        enemyRespawnTimer -= Time.deltaTime;

                        respawnSeconds = (int)(enemyRespawnTimer % 60);

                        if (enemyRespawnTimer <= 0)
                        {
                            SpawnEnemy();
                            enemyRespawnTimer = newEnemyTimer;
                        }
                    }

                    else
                    {
                        enemyRespawnTimer -= Time.deltaTime;

                        respawnSeconds = (int)(enemyRespawnTimer % 60);

                        if (enemyRespawnTimer <= 0)
                        {
                            SpawnEnemy();
                            enemyRespawnTimer = newEnemyTimer;
                        }
                    }
                    
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        enemySpawn = new Vector3(Random.Range(80, -80), 1, (Random.Range(80, -80)));
        Instantiate(Enemy, enemySpawn + tranDif, Quaternion.Euler(0, 0, 0));
        enemyCounter++;
    }

    public void ReduceEnemy()
    {
        enemyCounter--;
        enemyCounter--;
    }

    public void ActivateMagnetPowerUp()
    {
        isMagnetPowerUpActive = true;
    }

    public void ActivateTripleBulletPowerUp()
    {
        isTripleBulletPowerUpActive = true;
    }

    public void DebugSpawnMagnetPowerUp()
    {
        Instantiate(MagnetPowerUp, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        spawnMagnetPowerUpNow = false;
    }

    public void SpawnMagnetPowerUp()
    {
        Instantiate(MagnetPowerUp, new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)), Quaternion.Euler(0, 0, 0));
    }

    public void DebugSpawnTripleBulletPowerUp()
    {
        Instantiate(TripleBulletPowerUp, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        spawnTripleBulletPowerUpNow = false;
    }

    public void SpawnTripleBulletPowerUp()
    {
        Instantiate(TripleBulletPowerUp, new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)), Quaternion.Euler(0, 0, 0));
    }

}
