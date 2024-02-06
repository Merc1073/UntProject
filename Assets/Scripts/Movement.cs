using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //public GameObject clone;
    //public Quaternion rotation;

    //public Vector3 tranDif;

    public float forceMultiplier;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if(Input.GetMouseButton(0))
        {
            while(true)
            {
                RaycastHit hit;

                if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
                {
                    //clone = Instantiate(clone, hit.point + tranDif, rotation);
                    rb.AddForce(transform.position + hit.point * forceMultiplier * Time.deltaTime);
                }
            }

        }

    }
}
