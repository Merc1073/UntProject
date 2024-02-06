using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //public GameObject clone;
    //public Quaternion rotation;

    public Vector3 tranDif;

    public float forceMultiplier;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {


        //if(rb.velocity.magnitude > topSpeed)
        //{
        //    rb.velocity = rb.velocity.normalized;
        //}

                
        if(Input.GetMouseButton(0))
        {

            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {

                Vector3 directionToMouse = transform.position - hit.point;
                directionToMouse = directionToMouse.normalized * forceMultiplier;

                //clone = Instantiate(clone, hit.point + tranDif, rotation);
                rb.AddForce(-directionToMouse + tranDif * Time.deltaTime);
            }
            

        }

    }
}
