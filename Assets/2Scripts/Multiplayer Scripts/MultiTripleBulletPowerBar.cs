using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;

public class MultiTripleBulletPowerBar : NetworkBehaviour
{

    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;

    [ClientRpc]
    public void UpdateTripleBarClientRpc(float maxPower, float currentPower)
    {

        image.fillAmount = currentPower / maxPower;

    }
}
