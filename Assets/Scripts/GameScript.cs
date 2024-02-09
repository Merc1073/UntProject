using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] Text enemiesKilled;
    [SerializeField] Text coinsCollected;
    [SerializeField] Text totalScore;
    [SerializeField] Text magnetText;
    [SerializeField] Text tripleBulletText;

    [Header("Positions")]
    public Vector3 playerSpawn;
    public Vector3 enemySpawn;
    public Vector3 tranDif;

    public Vector3 bulletPointSpawn;
    public Vector3 reticlePointSpawn;

    public Vector3 magnetPowerUpSpawn;
    public Vector3 tripBulletPowerUpSpawn;

    [Header("Clock Timer")]
    [SerializeField] float totalTime;
    [SerializeField] int seconds;
    [SerializeField] int respawnSeconds;

    [Header("Powerups Respawn Timers")]
    [SerializeField] float originalTimerSpawnMagnetPowerUp;
    [SerializeField] float magnetRespawnTimer;

    [SerializeField] float originalTimerSpawnTripleBulletPowerUp;
    [SerializeField] float tripleBulletRespawnTimer;

    [Header("Powerups Lasting Timers")]
    [SerializeField] float magnetPowerUpTime;
    [SerializeField] float originalMagnetPowerUpTime;

    [SerializeField] float tripleBulletPowerUpTime;
    [SerializeField] float originalTripleBulletPowerUpTime;

    [Header("Enemy Respawn Timers")]
    public float enemyRespawnTimer;
    public float newEnemyTimer;

    [Header("Enemy Variables")]
    public float increaseInGlobalEnemyForceMultiplier;
    public float globalEnemyForceMultiplier;

    [Header("Coins and Score")]
    public float coinCount;
    public float scoreCount;
    public float addedEnemyScore;
    public float scoreMultiplier;
    public int decimalPlaces;

    [Header("Booleans")]
    public bool enemyFull = false;
    public bool keepReducingSpawnTimer = false;

    public bool canSpawnEnemies = false;
    public bool canSpawnMagnetPowerUp = false;
    public bool canSpawnTripleBulletPowerUp = false;

    public int enemyCounter;
    public int enemyKillCounter;
    public int maxEnemies;

    public bool spawnMagnetPowerUpNow = false;
    public bool spawnTripleBulletPowerUpNow = false;

    public string scoreCountRounded;
    public string newEnemyTimerRounded;
    public string magnetTimerRounded;
    public string tripleBulletTimerRounded;



    void Start()
    {

        if(SceneManager.GetActiveScene().name == "Rapid Fire")
        {
            Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
            Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
            Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));
        }

        else
        {
            Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
            Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));
        }

        playerScript = FindObjectOfType<MainPlayer>();
        bulletReticle = FindObjectOfType<BulletPoint>();


    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        scoreCount = coinCount * scoreMultiplier + addedEnemyScore;

        scoreCountRounded = scoreCount.ToString("F" + decimalPlaces);
        newEnemyTimerRounded = newEnemyTimer.ToString("F" + decimalPlaces);
        magnetTimerRounded = magnetPowerUpTime.ToString("F" + decimalPlaces);
        tripleBulletTimerRounded = tripleBulletPowerUpTime.ToString("F" + decimalPlaces);


        if (isGameModeRapidFire == true)
        {
            globalEnemyForceMultiplier += Time.deltaTime * increaseInGlobalEnemyForceMultiplier;
        }

        if (spawnMagnetPowerUpNow == true)
        {
            DebugSpawnMagnetPowerUp();
        }

        if (spawnTripleBulletPowerUpNow == true)
        {
            DebugSpawnTripleBulletPowerUp();
        }

        if(isMagnetPowerUpActive == true)
        {
            magnetPowerUpTime -= Time.deltaTime;

            if(magnetPowerUpTime <= 0f)
            {
                magnetPowerUpTime = 0f;
                isMagnetPowerUpActive = false;
            }

        }

        if (isTripleBulletPowerUpActive == true)
        {
            tripleBulletPowerUpTime -= Time.deltaTime;

            if (tripleBulletPowerUpTime <= 0f)
            {
                tripleBulletPowerUpTime = 0f;
                isTripleBulletPowerUpActive = false;
            }

        }


        if (playerScript != null)
        {

            totalTime += Time.deltaTime;
            seconds = (int)(totalTime);

            fireRateText.text = "Fire Rate: " + bulletReticle.roundsPerSecond.ToString() + " / Second";
            respawnTimerText.text = "Enemy spawns every " + newEnemyTimerRounded + " Seconds";
            enemyCountText.text = "Total Enemies in arena: " + enemyCounter.ToString();
            timerText.text = "Timer: " + seconds.ToString();
            enemiesKilled.text = "Enemies defeated: " + enemyKillCounter.ToString();
            coinsCollected.text = "Total of Orbs Collected: " + coinCount.ToString();
            totalScore.text = "Total Score: " + scoreCountRounded;
            magnetText.text = "Magnet Timer: " + magnetTimerRounded;
            tripleBulletText.text = "Triple Bullet Timer: " + tripleBulletTimerRounded;

            if (canSpawnMagnetPowerUp == true)
            {
                magnetRespawnTimer -= Time.deltaTime;

                if (magnetRespawnTimer <= 0)
                {
                    SpawnMagnetPowerUp();

                    magnetRespawnTimer = originalTimerSpawnMagnetPowerUp;
                }
            }

            if (canSpawnTripleBulletPowerUp == true)
            {
                tripleBulletRespawnTimer -= Time.deltaTime;

                if (tripleBulletRespawnTimer <= 0)
                {
                    SpawnTripleBulletPowerUp();

                    tripleBulletRespawnTimer = originalTimerSpawnTripleBulletPowerUp;
                }
            }

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

                    if (keepReducingSpawnTimer == true)
                    {
                        if(newEnemyTimer > 5f)
                        {
                            newEnemyTimer -= 0.1f * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemy();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }
                        
                        else if(newEnemyTimer > 2.5f)
                        {
                            newEnemyTimer -= 0.05f * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemy();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if(newEnemyTimer > 1.25f)
                        {
                            newEnemyTimer -= 0.025f * Time.deltaTime;
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
                            newEnemyTimer -= 0.00125f * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemy();
                                enemyRespawnTimer = newEnemyTimer;
                            }
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
        addedEnemyScore += 50f;
        enemyKillCounter++;
        enemyCounter--;
    }

    public void ActivateMagnetPowerUp()
    {
        magnetPowerUpTime = originalMagnetPowerUpTime;
        isMagnetPowerUpActive = true;
    }

    public void ActivateTripleBulletPowerUp()
    {
        tripleBulletPowerUpTime = originalTripleBulletPowerUpTime;
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
