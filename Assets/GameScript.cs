using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public GameObject Player;
    public GameObject Enemy;
    public GameObject BulletPoint;

    public Vector3 playerSpawn;
    public Vector3 enemySpawn;
    public Vector3 bulletPointSpawn;



    void Start()
    {
        Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(Enemy, enemySpawn, Quaternion.Euler(0, 0, 0));
        Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
    }

    void Update()
    {
        
    }
}
