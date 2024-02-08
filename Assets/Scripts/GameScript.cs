using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{

    public GameObject Player;
    public GameObject Enemy;
    public GameObject BulletPoint;
    public GameObject Reticle;

    private BulletPoint bulletReticle;

    [SerializeField] Text fireRateText, respawnTimerText, enemyCountText, timerText;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;
    public Vector3 bulletPointSpawn;
    public Vector3 reticlePointSpawn;

    float totalTime;

    int seconds;
    int respawnSeconds;

    public float respawnTimer;
    public float newTimer;

    public bool enemyFull = false;
    public bool canSpawnEnemies = false;

    public int enemyCounter;
    public int maxEnemies;



    void Start()
    {
        Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));

        bulletReticle = FindObjectOfType<BulletPoint>();

    }

    void Update()
    {

        totalTime += Time.deltaTime;
        seconds = (int)(totalTime);

        

        fireRateText.text = "Fire Rate: " + bulletReticle.roundsPerSecond.ToString();
        respawnTimerText.text = "Enemy Respawn in: " + respawnSeconds.ToString();
        enemyCountText.text = "Total Enemies in arena: " + enemyCounter.ToString();
        timerText.text = "Timer: " + seconds.ToString();

        if(canSpawnEnemies == true)
        {

            if(enemyCounter >= maxEnemies)
            {
                enemyFull = true;
            }

            if(enemyCounter < maxEnemies)
            {
                enemyFull = false; 
            }

            if(enemyFull == false)
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
        }
    }

    public void SpawnEnemy()
    {
        enemySpawn = new Vector3(Random.Range(10, -10), 1, (Random.Range(10, -10)));
        Instantiate(Enemy, enemySpawn, Quaternion.Euler(0, 0, 0));
        enemyCounter++;
    }

    public void ReduceEnemy()
    {
        enemyCounter--;
    }

}
