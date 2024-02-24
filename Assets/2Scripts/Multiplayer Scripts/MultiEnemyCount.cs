using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemyCount : MonoBehaviour
{
    public List<GameObject> allEnemies = new();
    public int enemyCount;


    private void Update()
    {
        enemyCount = allEnemies.Count;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MultiEnemy>())
        {
            allEnemies.Add(other.gameObject);
        }
    }
}
