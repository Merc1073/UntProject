using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCoinCount : MonoBehaviour
{
    public List<GameObject> allCoins = new();
    public int coinCount;


    private void Update()
    {
        coinCount = allCoins.Count;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MultiCoin>())
        {
            allCoins.Add(other.gameObject);
        }
    }
}
