using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiHealthState : NetworkBehaviour
{

    [HideInInspector]
    public NetworkVariable<float> HealthPoint = new NetworkVariable<float>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        HealthPoint.Value = 5f;
    }

}
