using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiMainPlayerCollision : NetworkBehaviour
{

    [SerializeField] GameObject playerCube;

    private void Update()
    {
        transform.position = playerCube.transform.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!IsServer) return;
        if (collider.CompareTag("MultiBullet"))
        {
            GetComponentInParent<MultiHealthState>().HealthPoint.Value -= 1f;
        }
    }

}
