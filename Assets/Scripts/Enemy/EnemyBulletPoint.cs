using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPoint : MonoBehaviour
{

    public float fireRate;
    public float fireRateCooldown;

    public float rotateVelocity;
    public float rotateSpeedMovement;

    GameObject player;
    public GameObject enemyBullet;

    public bool canFire = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    private void Update()
    {
        if(player != null)
        {
            fireRate += Time.deltaTime;

            if (fireRate >= fireRateCooldown)
            {
                canFire = true;
                fireRate = fireRateCooldown;
            }

            if (canFire == true)
            {
                fireRate = 0;

                Quaternion rotationToLookAt = Quaternion.LookRotation(player.transform.position - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                transform.eulerAngles = new Vector3(0, rotationY, 0);

                GameObject clone;

                clone = Instantiate(enemyBullet, transform.position, rotationToLookAt);

                canFire = false;
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position;
        //transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
