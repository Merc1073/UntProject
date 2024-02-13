using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
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
            Destroy(player.gameObject);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.Singleton.StartClient();
            Destroy(player.gameObject);
        }
    }


}
