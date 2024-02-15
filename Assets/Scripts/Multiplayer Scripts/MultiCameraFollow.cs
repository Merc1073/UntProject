using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiCameraFollow : NetworkBehaviour
{
    [SerializeField]
    private GameObject multiPlayer;

    public float smoothSpeed;
    public Vector3 offset;

    //bool ranOnce = false;

    private void Start()
    {
        transform.position = new Vector3(0, 60f, 0);
    }

    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(IsOwner);
        base.OnNetworkSpawn();
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
