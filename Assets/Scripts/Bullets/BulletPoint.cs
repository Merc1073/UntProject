using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.UI;
using UnityEngine;



public class BulletPoint : MonoBehaviour
{


    private GameObject playerObject;
    public GameObject bullet;
    private GameObject reticle;

    private MainPlayer playerScript;

    public LayerMask groundMask;

    public AudioSource src;
    public AudioClip pewSound;

    public float rotateVelocity;
    public float rotateSpeedMovement;

    public float fireRate;
    public float maxFireRate;
    public float fireRateCooldown;

    public bool canFire = false;

    public Vector3 tranDif;


    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        reticle = GameObject.FindGameObjectWithTag("Reticle");

        playerScript = FindObjectOfType<MainPlayer>();

        src = FindObjectOfType<AudioSource>();
    }

    void Update()
    {
        if(playerObject != null)
        {
            transform.position = playerObject.transform.position + tranDif;

            fireRate += Time.deltaTime;

            Debug.Log(maxFireRate);

            fireRateCooldown = maxFireRate;

            if (fireRate >= fireRateCooldown)
            {
                canFire = true;
                fireRate = fireRateCooldown;
            }


            if (Input.GetMouseButton(1) && canFire == true)
            {

                //src.clip = pewSound;
                //src.pitch = Random.Range(0.3f, 1f);
                src.volume = 1f;
                src.PlayOneShot(pewSound);

                fireRate = 0;

                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
                {
                    Quaternion rotationToLookAt = Quaternion.LookRotation(reticle.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
                    transform.eulerAngles = new Vector3(0, rotationY, 0);

                    GameObject clone;

                    clone = Instantiate(bullet, transform.position, rotationToLookAt);

                }

                canFire = false;

            }
        }
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    public void IncreaseFireRate(float rateOfFire)
    {
        maxFireRate -= rateOfFire;
    }

}
