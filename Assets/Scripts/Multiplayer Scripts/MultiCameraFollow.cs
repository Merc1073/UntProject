using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiCameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject multiPlayer;

    public float smoothSpeed;
    public Vector3 offset;

    //bool ranOnce = false;

    private void Start()
    {

    }

    private void FixedUpdate()
    {

        if(multiPlayer)
        {
            Vector3 desiredPosition = multiPlayer.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }

        else
        {
            Debug.Log("player doesn't exist for some reason but should right?");
        }

    }
}
