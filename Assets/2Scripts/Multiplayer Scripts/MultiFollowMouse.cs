using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MultiFollowMouse : NetworkBehaviour
{
    public Vector3 tranDif;

    public LayerMask groundMask;

    [SerializeField]
    private Camera cameraPlayer;

    void Update()
    {
        //if(!IsOwner) return;

        RaycastHit hit;

        if (Physics.Raycast(cameraPlayer.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        {
            transform.position = hit.point + tranDif;
        }
    }

    //public void DestroyObj()
    //{
    //    Destroy(gameObject);
    //}

    //public override void OnNetworkSpawn()
    //{
    //    gameObject.SetActive(IsOwner);
    //    base.OnNetworkSpawn();
    //}

    //[ServerRpc]
    //private void FollowMouseServerRpc()
    //{
        
    //}
}
