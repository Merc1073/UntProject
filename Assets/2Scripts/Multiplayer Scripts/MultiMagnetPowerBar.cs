using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;

public class MultiMagnetPowerBar : NetworkBehaviour
{

    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;

    [ClientRpc]
    public void UpdateMagnetBarClientRpc(float maxPower, float currentPower)
    {

        image.fillAmount = currentPower / maxPower;

    }

}
