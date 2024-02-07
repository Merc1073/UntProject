using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public GameObject Player;
    public GameObject Enemy;
    public GameObject BulletPoint;
    public GameObject Reticle;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;
    public Vector3 bulletPointSpawn;
    public Vector3 reticlePointSpawn;

    public float timer;
    public float newTimer;

    public bool enemyFull = false;
    public bool canSpawnEnemies = false;

    public int counter;
    public int maxEnemies;



    void Start()
    {
        Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        //Instantiate(Enemy, enemySpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));

    }

    void Update()
    {

        if(canSpawnEnemies == true)
        {

            if(counter >= maxEnemies)
            {
                enemyFull = true;
            }

            if(counter < maxEnemies)
            {
                enemyFull = false; 
            }

            if(enemyFull == false)
            {
            
                newTimer -= 0.0001f;
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    SpawnEnemy();
                    timer = newTimer;
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        enemySpawn = new Vector3(Random.Range(10, -10), 1, (Random.Range(10, -10)));
        Instantiate(Enemy, enemySpawn, Quaternion.Euler(0, 0, 0));
        counter++;
    }

    public void ReduceEnemy()
    {
        counter--;
    }

}
