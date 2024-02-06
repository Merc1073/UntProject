using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BulletPoint : MonoBehaviour
{


    public GameObject player;
    public GameObject bullet;

    public float rotateVelocity;
    public float rotateSpeedMovement;

    public Vector3 tranDif;


    void Start()
    {
        
    }

    void Update()
    {

        transform.position = player.transform.position + tranDif;
        

        if(Input.GetMouseButtonDown(1))
        {

            RaycastHit hit;

            

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                transform.eulerAngles = new Vector3(0, rotationY, 0);

                GameObject clone;

                clone = Instantiate(bullet, transform.position, rotationToLookAt);
                
            }
        }

    }
}
