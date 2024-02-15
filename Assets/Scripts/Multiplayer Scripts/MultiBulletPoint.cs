using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MultiBulletPoint : NetworkBehaviour
{
    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject reticle;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Camera cameraPlayer;

    //Camera customCamera;
    //private MainPlayer playerScript;

    GameScript gameScript;

    public LayerMask groundMask;

    public AudioSource src;
    public AudioClip pewSound;

    public float rotateVelocity;
    public float rotateSpeedMovement;

    public Quaternion rotationToLookAt;

    public float fireRate;
    public float maxFireRate;
    public float fireRateCooldown;
    public float fireRateMultiplier;

    public float roundsPerSecond;
    public int decimalPlaces;

    public bool canFire = false;

    public Vector3 tranDif;

    [SerializeField]
    private List<GameObject> spawnedMultiBullets = new List<GameObject>();


    void Start()
    {

        //DontDestroyOnLoad(gameObject);

        //reticle = GameObject.FindGameObjectWithTag("Reticle");

        //playerScript = FindObjectOfType<MainPlayer>();
        gameScript = FindObjectOfType<GameScript>();

        src = GetComponent<AudioSource>();

    }

    //public override void OnNetworkSpawn()
    //{

    //    base.OnNetworkSpawn();
    //}

    void Update()
    {
        if (!IsOwner) return;

        transform.position = player.transform.position + tranDif;

        fireRate = 1 / fireRateMultiplier;

        fireRateCooldown -= Time.deltaTime;

        roundsPerSecond = fireRateMultiplier;

        roundsPerSecond = Mathf.Round(roundsPerSecond * Mathf.Pow(10, decimalPlaces)) / Mathf.Pow(10, decimalPlaces);

        if (fireRateMultiplier > maxFireRate)
        {
            fireRateMultiplier = maxFireRate;
        }

        if (fireRateCooldown < 0f)
        {
            fireRateCooldown = 0f;
        }

        //if (player.GetComponent<MultiMainPlayer>().canMove == true)
        //{

        //if (!IsOwner) return;

        if (Input.GetMouseButton(0) && fireRateCooldown <= 0 && gameScript.isTripleBulletPowerUpActive == false)
        {
            FireNormalBullet();
        }

        if (Input.GetMouseButton(0) && fireRateCooldown <= 0 && gameScript.isTripleBulletPowerUpActive == true)
        {
            FireTripleBullet();
        }
        //}


    }


    private void FireNormalBullet()
    {

        src.clip = pewSound;
        src.volume = 0.6f;
        src.pitch = Random.Range(0.6f, 0.8f);
        src.PlayOneShot(pewSound);

        RaycastHit hit;

        if (Physics.Raycast(cameraPlayer.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        {
            
            CreateBulletServerRpc();

        }

        fireRateCooldown = fireRate;

        canFire = false;
    }

    private void FireTripleBullet()
    {

        src.clip = pewSound;
        src.volume = 0.6f;
        src.pitch = Random.Range(1.0f, 1.2f);
        src.PlayOneShot(pewSound);

        RaycastHit hit;

        if (Physics.Raycast(cameraPlayer.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        {
            Quaternion rotationToLookAt = Quaternion.LookRotation(reticle.transform.position - transform.position);

            GameObject clone1, clone2, clone3;

            clone1 = Instantiate(bullet, transform.position, rotationToLookAt);

            clone2 = Instantiate(bullet, transform.position, rotationToLookAt);
            clone2.transform.Rotate(0, 10, 0);

            clone3 = Instantiate(bullet, transform.position, rotationToLookAt);
            clone3.transform.Rotate(0, -10, 0);

        }

        fireRateCooldown = fireRate;

        canFire = false;
    }

    [ServerRpc]
    private void CreateBulletServerRpc(/*ServerRpcParams serverRpcParams = default*/)
    {
        rotationToLookAt = Quaternion.LookRotation(reticle.transform.position - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);

        GameObject clone = Instantiate(bullet, transform.position, rotationToLookAt);
        spawnedMultiBullets.Add(clone);

        clone.GetComponent<MultiBullet>().parent = this;
        clone.GetComponent<NetworkObject>().Spawn();
        //clone.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyBulletServerRpc()
    {
        GameObject toDestroy = spawnedMultiBullets[0];
        toDestroy.GetComponent<NetworkObject>().Despawn();
        spawnedMultiBullets.Remove(toDestroy);
        Destroy(toDestroy);
    }

    //public void DestroyObj()
    //{
    //    Destroy(gameObject);
    //}

    public void IncreaseFireRate(float addedFireRate)
    {
        fireRateMultiplier += addedFireRate;
    }
}
