using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiHealthState : NetworkBehaviour
{

    public NetworkVariable<float> HealthPoint = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        HealthPoint.Value = 5f;
    }


    [ServerRpc(RequireOwnership = false)]
    public void DecreaseHealthServerRpc(float valueToDecrease)
    {
        HealthPoint.Value -= valueToDecrease;
    }

}
