using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{

    public Vector3 tranDif;

    public LayerMask groundMask;

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        {
            transform.position = hit.point + tranDif;
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

}
