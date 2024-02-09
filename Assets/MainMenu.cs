using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    Transform startButton;
    Transform gameModesButton;
    Transform quitButton;

    public GameObject player;
    GameScript gameScript;

    public float startDistanceToPlayer;
    public float gameModeDistanceToPlayer;
    public float quitDistanceToPlayer;

    void Start()
    {

        gameScript = FindObjectOfType<GameScript>();
        
    }

    void Update()
    {

        startButton = transform.GetChild(0);
        gameModesButton = transform.GetChild(1);
        quitButton = transform.GetChild(2);

        startDistanceToPlayer = Vector3.Distance(startButton.transform.position, player.transform.position);
        gameModeDistanceToPlayer = Vector3.Distance(gameModesButton.position, player.transform.position);
        quitDistanceToPlayer = Vector3.Distance(quitButton.position, player.transform.position);

        if(startDistanceToPlayer <= 2f)
        {
            //gameScript.isGameModeRapidFire = true;

            //gameScript.canSpawnEnemies = true;
            //gameScript.canSpawnMagnetPowerUp = true;
            //gameScript.canSpawnTripleBulletPowerUp = true;
            //gameScript.keepReducingSpawnTimer = true;

            gameScript.isGameModeRapidFire = true;
            SceneManager.LoadScene("Rapid Fire");
        }

    }

}
