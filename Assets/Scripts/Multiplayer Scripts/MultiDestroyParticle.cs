using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiDestroyParticle : NetworkBehaviour
{
    [SerializeField] private float timer;
    [SerializeField] private float timeUntilDestroyed;

    void Update()
    {
        if (transform.childCount <= 0)
        {
            timer += Time.deltaTime;

            if (timer >= timeUntilDestroyed)
            {
                DestroyParticleServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyParticleServerRpc()
    {
        GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }

}
