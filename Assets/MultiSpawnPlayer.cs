using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiSpawnPlayer : NetworkBehaviour
{

    public GameObject mainPlayer;
    private bool hasExecuted = false;

    private void Start()
    {

        GetComponent<NetworkObject>().Spawn();

        if(!hasExecuted)
        {
            GameObject player = Instantiate(mainPlayer, new Vector3(0, 1f, 0), Quaternion.identity);
            player.GetComponentInParent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
            hasExecuted = true;
            Destroy(gameObject);
            GetComponent<NetworkObject>().Despawn();
        }
    }

}
