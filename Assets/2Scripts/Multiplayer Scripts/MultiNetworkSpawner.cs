using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiNetworkSpawner : NetworkBehaviour
{

    [SerializeField] private GameObject multiMainMenu;
    [SerializeField] private GameObject multiGameScript;

    private void Start()
    {
        SpawnNetworkObjectsServerRpc();
    }


    [ServerRpc]
    private void SpawnNetworkObjectsServerRpc()
    {
        Instantiate(multiMainMenu, transform.position, Quaternion.identity);
        Instantiate(multiGameScript, transform.position, Quaternion.identity);

        multiMainMenu.GetComponent<NetworkObject>().Spawn();
        multiGameScript.GetComponent<NetworkObject>().Spawn();
    }

}
