using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiMagnetCount : MonoBehaviour
{
    public List<GameObject> allMagnets = new();
    public int magnetCount;


    private void Update()
    {
        magnetCount = allMagnets.Count;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MultiMagnetPowerUp>())
        {
            allMagnets.Add(other.gameObject);
        }
    }
}
