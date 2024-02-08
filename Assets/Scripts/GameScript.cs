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

    private MainPlayer playerScript;
    private BulletPoint bulletReticle;

    [Header("Special Booleans")]
    public bool isMagnetPowerUpActive = false;

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

    [Header("Number Variables")]
    [SerializeField] float totalTime;
    [SerializeField] int seconds;
    [SerializeField] int respawnSeconds;

    public float respawnTimer;
    public float newTimer;

    [Header("Booleans")]
    public bool enemyFull = false;
    public bool canSpawnEnemies = false;
    public bool keepReducingSpawnTimer = false;

    public int enemyCounter;
    public int maxEnemies;



    void Start()
    {
        Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(MagnetPowerUp, magnetPowerUpSpawn, Quaternion.Euler(0, 0, 0));

        playerScript = FindObjectOfType<MainPlayer>();
        bulletReticle = FindObjectOfType<BulletPoint>();

    }

    void Update()
    {
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
                        newTimer -= 0.0001f;
                        respawnTimer -= Time.deltaTime;

                        respawnSeconds = (int)(respawnTimer % 60);

                        if (respawnTimer <= 0)
                        {
                            SpawnEnemy();
                            respawnTimer = newTimer;
                        }
                    }

                    else
                    {
                        respawnTimer -= Time.deltaTime;

                        respawnSeconds = (int)(respawnTimer % 60);

                        if (respawnTimer <= 0)
                        {
                            SpawnEnemy();
                            respawnTimer = newTimer;
                        }
                    }
                    
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        enemySpawn = new Vector3(Random.Range(40, -40), 1, (Random.Range(40, -40)));
        Instantiate(Enemy, enemySpawn + tranDif, Quaternion.Euler(0, 0, 0));
        enemyCounter++;
    }

    public void ReduceEnemy()
    {
        enemyCounter--;
    }

    public void ActivateMagnetPowerUp()
    {
        isMagnetPowerUpActive = true;
    }

}
