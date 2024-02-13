using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private GameObject player;
    private GameObject multiPlayer;

    public float smoothSpeed;
    public Vector3 offset;

    bool ranOnce = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if(!player && !ranOnce)
        {
            multiPlayer = GameObject.FindGameObjectWithTag("MultiPlayer");
            ranOnce = true;
        }

        if (player)
        {
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }  

        else if(multiPlayer != null)
        {
            Vector3 desiredPosition = multiPlayer.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
