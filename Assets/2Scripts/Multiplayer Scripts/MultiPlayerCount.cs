using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCount : MonoBehaviour
{

    public List<GameObject> allPlayers = new();

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
