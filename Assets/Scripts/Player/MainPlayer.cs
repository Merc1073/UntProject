using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayer : MonoBehaviour
{

    public Vector3 tranDif;

    public LayerMask groundMask;

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

            RaycastHit hit;

            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
            {

                Vector3 directionToMouse = transform.position - hit.point;
                directionToMouse = directionToMouse.normalized * forceMultiplier;

                //clone = Instantiate(clone, hit.point + tranDif, rotation);
                rb.AddForce(-directionToMouse + tranDif * Time.deltaTime);
            }
            

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

}
