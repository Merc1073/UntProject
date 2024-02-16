using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiEnemyDetection : MonoBehaviour
{
    public List<Transform> enemy;

    public Transform targetEnemy;

    public List<GameObject> enemyObject;

    public GameObject targetEnemyObject;


    private void Start()
    {
        targetEnemy = null;
        targetEnemyObject = null;
    }


    private void Update()
    {
        enemy.RemoveAll(i => i == null);
        enemyObject.RemoveAll(i => i == null);

        if (enemy.Count != 0)
        {
            targetEnemy = GetClosestTransform(enemy);
            targetEnemyObject = GetClosestObject(enemyObject);
        }

        if (enemy.Count == 0)
        {
            targetEnemy = null;
            targetEnemyObject = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MultiEnemy"))
        {
            enemy.Add(other.transform);
            enemyObject.Add(other.gameObject);
        }
    }

    Transform GetClosestTransform(List<Transform> items)
    {

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Transform potentialTarget in items)
        {

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }

        }

        return bestTarget;
    }


    GameObject GetClosestObject(List<GameObject> items)
    {

        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject potentialTarget in items)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
