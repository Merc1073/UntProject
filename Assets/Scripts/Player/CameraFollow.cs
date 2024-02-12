using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    GameScript gameScript;
    Transform player;

    public float smoothSpeed;
    public Vector3 offset;

    public bool doesPlayerExist = false;

    private void Start()
    {
        gameScript = FindObjectOfType<GameScript>();

        player = gameScript.GetComponent<Detection>().targetPlayer;
    }

    private void Update()
    {
        if (!player && !doesPlayerExist)
        {
            player = gameScript.GetComponent<Detection>().targetPlayer;
            doesPlayerExist = true;
        }
    }

    private void FixedUpdate()
    {

        if(player)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
