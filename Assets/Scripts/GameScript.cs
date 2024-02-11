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

    public FadeTransition fader;

    [Header("Special Booleans")]

    //public bool menuGameModeRapidFire = false;
    //public bool menuGameModeGrowth = false;

    public bool isGameModeRapidFire = false;
    public bool isGameModeGrowing = false;

    public bool hasRapidFireModeStarted = false;
    public bool isCurrentSceneRapidFireMode = false;

    public bool isMagnetPowerUpActive = false;
    public bool isTripleBulletPowerUpActive = false;

    public bool hasGrowingModeStarted = false;

    public bool hasSceneBeenLoaded = false;

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
    public Vector3 playerMenuSpawn;
    public Vector3 playerRapidFireSpawn;

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

    [Header("Decimal Places")]
    public int genericDecimalPlaces;
    public int scoreDecimalPlaces;
    public int newEnemyDecimalPlaces;

    [Header("Enemy Timer Stages")]
    [SerializeField] float stage1;
    [SerializeField] float stage2;
    [SerializeField] float stage3;
    [SerializeField] float stage4;
    [SerializeField] float stage5;
    [SerializeField] float stage6;

    [Header("Other Variables")]
    [SerializeField] float enemySpawnRange;
    [SerializeField] float magnetSpawnRange;
    [SerializeField] float tripleBulletSpawnRange;
    [SerializeField] float invincibilitySpawnRange;

    [Header("Gamemode Booleans")]
    public bool skipTutorial = false;

    [Header("Player Booleans")]
    public bool isPlayerInvincible = false;

    [Header("Enemy Booleans")]
    public bool enemyFull = false;
    public bool keepReducingSpawnTimer = false;

    [Header("Spawn Booleans")]
    public bool canSpawnEnemies = false;
    public bool canSpawnMagnetPowerUp = false;
    public bool canSpawnTripleBulletPowerUp = false;

    [Header("Enemy variables")]
    public int enemyCounter;
    public int enemyKillCounter;
    public int maxEnemies;

    [Header("Debug spawn in objects")]
    public bool spawnEnemyNow = false;
    public bool spawnMagnetPowerUpNow = false;
    public bool spawnTripleBulletPowerUpNow = false;

    string scoreCountRounded;
    string newEnemyTimerRounded;
    string magnetTimerRounded;
    string tripleBulletTimerRounded;



    void Start()
    {

        DontDestroyOnLoad(gameObject);

        //if (SceneManager.GetActiveScene().name == "Rapid Fire")
        //{
        //    //isCurrentSceneRapidFireMode = true;

        //    Debug.Log("It's working");

        //    Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        //    Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        //    Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));

        //    isGameModeRapidFire = true;

        //    stage1 = stage1ToSet;
        //    stage2 = stage2ToSet;
        //    stage3 = stage3ToSet;
        //    stage4 = stage4ToSet;
        //    stage5 = stage5ToSet;
        //    stage6 = stage6ToSet;

        //}

        //else
        //{
            Instantiate(Player, playerMenuSpawn, Quaternion.Euler(0, 0, 0));
            Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
            Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));
        //}

        if(hasRapidFireModeStarted == false)
        {
            fireRateText.gameObject.SetActive(false);
            respawnTimerText.gameObject.SetActive(false);
            enemyCountText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            enemiesKilled.gameObject.SetActive(false);
            coinsCollected.gameObject.SetActive(false);
            totalScore.gameObject.SetActive(false);
            magnetText.gameObject.SetActive(false);
            tripleBulletText.gameObject.SetActive(false);
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


        if (SceneManager.GetActiveScene().name == "Rapid Fire" && hasSceneBeenLoaded == false)
        {
            isCurrentSceneRapidFireMode = true;

            playerScript = FindObjectOfType<MainPlayer>();
            bulletReticle = FindObjectOfType<BulletPoint>();

            playerScript.transform.position = playerRapidFireSpawn;

            isGameModeRapidFire = true;

            //stage1 = stage1ToSet;
            //stage2 = stage2ToSet;
            //stage3 = stage3ToSet;
            //stage4 = stage4ToSet;
            //stage5 = stage5ToSet;
            //stage6 = stage6ToSet;

            hasSceneBeenLoaded = true;

        }

        if (hasRapidFireModeStarted == true)
        {
            fireRateText.gameObject.SetActive(true);
            respawnTimerText.gameObject.SetActive(true);
            enemyCountText.gameObject.SetActive(true);
            timerText.gameObject.SetActive(true);
            enemiesKilled.gameObject.SetActive(true);
            coinsCollected.gameObject.SetActive(true);
            totalScore.gameObject.SetActive(true);
            //magnetText.gameObject.SetActive(true);
            //tripleBulletText.gameObject.SetActive(true);

            keepReducingSpawnTimer = true;

            canSpawnEnemies = true;
            canSpawnMagnetPowerUp = true;
            canSpawnTripleBulletPowerUp = true;


        }

        if (newEnemyTimer <= 0f)
        {
            newEnemyTimer = 0f;
        }

        scoreCount = coinCount * scoreMultiplier + addedEnemyScore;

        scoreCountRounded = scoreCount.ToString("F" + scoreDecimalPlaces);
        newEnemyTimerRounded = newEnemyTimer.ToString("F" + newEnemyDecimalPlaces);
        magnetTimerRounded = magnetPowerUpTime.ToString("F" + genericDecimalPlaces);
        tripleBulletTimerRounded = tripleBulletPowerUpTime.ToString("F" + genericDecimalPlaces);

        playerScript.GetComponentInChildren<MagnetPowerBar>().UpdateMagnetBar(originalMagnetPowerUpTime, magnetPowerUpTime);
        playerScript.GetComponentInChildren<TripleBulletPowerBar>().UpdateTripleBulletBar(originalTripleBulletPowerUpTime, tripleBulletPowerUpTime);

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

        if(spawnEnemyNow == true)
        {
            DebugSpawnEnemy();
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


        if (playerScript && hasRapidFireModeStarted == true)
        {


            totalTime += Time.deltaTime;
            seconds = (int)(totalTime);

            fireRateText.text = "Fire Rate: " + bulletReticle.roundsPerSecond.ToString() + " / Second";
            respawnTimerText.text = "Enemy spawns every " + newEnemyTimerRounded + " Seconds";
            enemyCountText.text = "Total Enemies in arena: " + enemyCounter.ToString();
            timerText.text = "Timer: " + seconds.ToString();
            enemiesKilled.text = "Enemies defeated: " + enemyKillCounter.ToString();
            coinsCollected.text = "Orbs Collected: " + coinCount.ToString();
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
                        
                        if(newEnemyTimer > 3.75f)
                        {
                            newEnemyTimer -= stage1 * Time.deltaTime;
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
                            newEnemyTimer -= stage2 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemy();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 1.25f)
                        {
                            newEnemyTimer -= stage3 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemy();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 0.625f)
                        {
                            newEnemyTimer -= stage4 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemy();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 0.3125f)
                        {
                            newEnemyTimer -= stage5 * Time.deltaTime;
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
                            newEnemyTimer -= stage6 * Time.deltaTime;
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

        //else
        //{
        //    Debug.Log("It's not working :/");
        //}

    } 
    
    public void DebugSpawnEnemy()
    {
        Instantiate(Enemy, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        enemyCounter++;
        spawnEnemyNow = false;
    }

    public void SpawnEnemy()
    {
        enemySpawn = new Vector3(Random.Range(enemySpawnRange, -enemySpawnRange), 1, (Random.Range(enemySpawnRange, -enemySpawnRange)));
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
        Instantiate(MagnetPowerUp, new Vector3(Random.Range(-magnetSpawnRange, magnetSpawnRange), 0, Random.Range(-magnetSpawnRange, magnetSpawnRange)), Quaternion.Euler(0, 0, 0));
    }

    public void DebugSpawnTripleBulletPowerUp()
    {
        Instantiate(TripleBulletPowerUp, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));

        spawnTripleBulletPowerUpNow = false;
    }

    public void SpawnTripleBulletPowerUp()
    {
        Instantiate(TripleBulletPowerUp, new Vector3(Random.Range(-tripleBulletSpawnRange, tripleBulletSpawnRange), 0, Random.Range(-tripleBulletSpawnRange, tripleBulletSpawnRange)), Quaternion.Euler(0, 0, 0));
    }

}
