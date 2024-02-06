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



    void Start()
    {
        Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Enemy, enemySpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));

    }

    void Update()
    {

        enemySpawn = new Vector3(Random.Range(10, -10), 1, (Random.Range(10, -10)));

        timer += Time.deltaTime;

        if(timer >= 3)
        {
            Instantiate(Enemy, enemySpawn, Quaternion.Euler(0, 0, 0));
            timer = 0;
        }


    }
}
