using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("MasterSinglePlayer");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            NetworkManager.Singleton.StartHost();
            if(player)
            {
                Destroy(player.gameObject);
            }

            SceneManager.LoadScene("Multi Main Menu");
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.Singleton.StartClient();
            Destroy(player.gameObject);
        }
    }

}
