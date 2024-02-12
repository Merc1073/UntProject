using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    public List<Transform> player;
    public List<Transform> enemy;

    public Transform targetPlayer;
    public Transform targetEnemy;

    public List<GameObject> playerObject;
    public List<GameObject> enemyObject;

    public GameObject targetPlayerObject;
    public GameObject targetEnemyObject;


    private void Start()
    {
        targetPlayer = null;
        targetEnemy = null;

        targetPlayerObject = null;
        targetEnemyObject = null;
    }


    private void Update()
    {
        player.RemoveAll(i => i == null);
        enemy.RemoveAll(i => i == null);

        playerObject.RemoveAll(i => i == null);
        enemyObject.RemoveAll(i => i == null);



        if(player.Count != 0)
        {
            targetPlayer = GetClosestTransform(player);
            targetPlayerObject = GetClosestObject(playerObject);
        }

        if(player.Count == 0)
        {
            targetPlayer = null;
            targetPlayerObject = null;
        }

        if(enemy.Count != 0)
        {
            targetEnemy = GetClosestTransform(enemy);
            targetEnemyObject = GetClosestObject(enemyObject);
        }

        if(enemy.Count == 0)
        {
            targetEnemy = null;
            targetEnemyObject = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player.Add(other.transform);
            playerObject.Add(other.gameObject);
        }

        if(other.gameObject.CompareTag("Enemy"))
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
