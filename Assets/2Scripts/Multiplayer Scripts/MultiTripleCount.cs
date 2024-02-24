using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTripleCount : MonoBehaviour
{
    public List<GameObject> allTriples = new();
    public int tripleCount;


    private void Update()
    {
        tripleCount = allTriples.Count;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MultiTripleBulletPowerUp>())
        {
            allTriples.Add(other.gameObject);
        }
    }
}
