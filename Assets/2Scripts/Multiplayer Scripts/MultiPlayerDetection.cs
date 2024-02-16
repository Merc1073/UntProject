using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiPlayerDetection : NetworkBehaviour
{
    public List<Transform> player;

    public Transform targetPlayer;

    public List<GameObject> playerObject;

    public GameObject targetPlayerObject;


    private void Start()
    {
        targetPlayer = null;
        targetPlayerObject = null;
    }


    private void Update()
    {
        player.RemoveAll(i => i == null);
        playerObject.RemoveAll(i => i == null);

        if (player.Count != 0)
        {
            targetPlayer = GetClosestTransform(player);
            targetPlayerObject = GetClosestObject(playerObject);
        }

        if (player.Count == 0)
        {
            targetPlayer = null;
            targetPlayerObject = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MultiPlayer"))
        {
            player.Add(other.transform);
            playerObject.Add(other.gameObject);
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
