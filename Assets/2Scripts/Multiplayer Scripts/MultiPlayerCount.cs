using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiPlayerCount : MonoBehaviour
{

    public List<GameObject> allPlayers = new();
    //public int playerCount;
    //public NetworkVariable<int> playerCount = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    //private void Start()
    //{
    //    playerCount.Value = 0;
    //}

    //private void Update()
    //{
    //    playerCount.Value = allPlayers.Count;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MultiMainPlayer>())
        {
            allPlayers.Add(other.gameObject);
        }
    }

    //private void Start()
    //{
    //    for(int i = 0; i < allPlayers.Count; i++)
    //    {
    //        allPlayers[i].name = "Player " + i.ToString();
    //    }
    //}

}
