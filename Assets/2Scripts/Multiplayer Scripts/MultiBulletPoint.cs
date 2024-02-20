using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    private GameObject bulletNoise;

    [SerializeField]
    private Camera cameraPlayer;

    [SerializeField]
    private GameObject magnetPowerBar;

    [SerializeField]
    private GameObject triplePowerBar;

    //[SerializeField]
    //private GameObject tripleBulletPowerBar;

    //Camera customCamera;
    //private MainPlayer playerScript;

    //GameScript gameScript;

    public LayerMask groundMask;

    //public AudioSource src;
    //public AudioClip pewSound;

    public float rotateVelocity;
    public float rotateSpeedMovement;

    public Quaternion rotationToLookAt;

    public float fireRate;
    public float maxFireRate;
    public float fireRateCooldown;
    public float fireRateMultiplier;

    public float roundsPerSecond;
    public int decimalPlaces;

    //public float originalMagnetPowerUpTime;
    //public float magnetPowerUpTime;

    public NetworkVariable<float> originalMagnetPowerUpTime = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> magnetPowerUpTime = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<float> originalTriplePowerUpTime = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> triplePowerUpTime = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    //public bool isMagnetPowerUpActive = false;
    //public bool hasMagnetTriggered = false;

    public NetworkVariable<bool> isMagnetPowerUpActive = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasMagnetTriggered = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<bool> isTriplePowerUpActive = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasTripleTriggered = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    //public bool isTripleBulletPowerUpActive = false;

    public bool canFire = false;

    public Vector3 tranDif;

    [SerializeField]
    private List<GameObject> spawnedMultiBullets = new List<GameObject>();


    void Start()
    {

        //DontDestroyOnLoad(gameObject);

        //reticle = GameObject.FindGameObjectWithTag("Reticle");

        //playerScript = FindObjectOfType<MainPlayer>();
        //gameScript = FindObjectOfType<GameScript>();

        //src = GetComponent<AudioSource>();

    }

    public override void OnNetworkSpawn()
    {
        originalMagnetPowerUpTime.Value = 5f;
        magnetPowerUpTime.Value = 0f;

        originalTriplePowerUpTime.Value = 5f;
        triplePowerUpTime.Value = 0f;

        isMagnetPowerUpActive.Value = false;
        hasMagnetTriggered.Value = false;

        isTriplePowerUpActive.Value = false;
        hasTripleTriggered.Value = false;

        base.OnNetworkSpawn();
    }

    void Update()
    {
        if (!IsOwner) return;

        //Debug.Log(isMagnetPowerUpActive);

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

        if (Input.GetMouseButton(0) && fireRateCooldown <= 0 && isTriplePowerUpActive.Value == false)
        {
            FireNormalBullet();
        }

        if (Input.GetMouseButton(0) && fireRateCooldown <= 0 && isTriplePowerUpActive.Value == true)
        {
            FireTripleBullet();
        }



        UpdateMagnetUIServerRpc();
        UpdateTripleUIServerRpc();



        if (hasMagnetTriggered.Value)
        {
            magnetPowerUpTime.Value = originalMagnetPowerUpTime.Value;
            hasMagnetTriggered.Value = false;
        }

        if (isMagnetPowerUpActive.Value)
        {

            magnetPowerUpTime.Value -= Time.deltaTime;

            if (magnetPowerUpTime.Value <= 0f)
            {
                magnetPowerUpTime.Value = 0f;
                isMagnetPowerUpActive.Value = false;
            }

        }



        if(hasTripleTriggered.Value)
        {
            triplePowerUpTime.Value = originalTriplePowerUpTime.Value;
            hasTripleTriggered.Value = false;
        }

        if(isTriplePowerUpActive.Value)
        {

            triplePowerUpTime.Value -= Time.deltaTime;

            if(triplePowerUpTime.Value <= 0f)
            {
                triplePowerUpTime.Value = 0f;
                isTriplePowerUpActive.Value = false;
            }

        }

    }


    private void FireNormalBullet()
    {

        CreateBulletNoiseServerRpc();

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

        CreateBulletNoiseServerRpc();

        RaycastHit hit;

        if (Physics.Raycast(cameraPlayer.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        {
            CreateTripleBulletServerRpc();
        }

        fireRateCooldown = fireRate;

        canFire = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateBulletServerRpc(/*ServerRpcParams serverRpcParams = default*/)
    {
        //if (!IsClient) return;

        rotationToLookAt = Quaternion.LookRotation(reticle.transform.position - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);

        GameObject clone = Instantiate(bullet, transform.position, rotationToLookAt);

        if (isMagnetPowerUpActive.Value)
        {
            clone.GetComponent<MultiBullet>().isMagnetPowerUpActive = true;
        }

        spawnedMultiBullets.Add(clone);

        clone.GetComponent<MultiBullet>().parent = this;
        clone.GetComponent<NetworkObject>().Spawn();
        //clone.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);

    }

    [ServerRpc]
    private void CreateTripleBulletServerRpc(/*ServerRpcParams serverRpcParams = default*/)
    {
        //if (!IsClient) return;

        rotationToLookAt = Quaternion.LookRotation(reticle.transform.position - transform.position);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));
        transform.eulerAngles = new Vector3(0, rotationY, 0);

        GameObject clone1, clone2, clone3;

        clone1 = Instantiate(bullet, transform.position, rotationToLookAt);

        clone2 = Instantiate(bullet, transform.position, rotationToLookAt);
        clone2.transform.Rotate(0, 10, 0);

        clone3 = Instantiate(bullet, transform.position, rotationToLookAt);
        clone3.transform.Rotate(0, -10, 0);


        if (isMagnetPowerUpActive.Value)
        {
            clone1.GetComponent<MultiBullet>().isMagnetPowerUpActive = true;
            clone2.GetComponent<MultiBullet>().isMagnetPowerUpActive = true;
            clone3.GetComponent<MultiBullet>().isMagnetPowerUpActive = true;
        }

        spawnedMultiBullets.Add(clone1);
        spawnedMultiBullets.Add(clone2);
        spawnedMultiBullets.Add(clone3);

        clone1.GetComponent<MultiBullet>().parent = this;
        clone2.GetComponent<MultiBullet>().parent = this;
        clone3.GetComponent<MultiBullet>().parent = this;

        clone1.GetComponent<NetworkObject>().Spawn();
        clone2.GetComponent<NetworkObject>().Spawn();
        clone3.GetComponent<NetworkObject>().Spawn();
        //clone.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);

    }

    [ServerRpc(RequireOwnership = false)]
    public void DestroyBulletServerRpc()
    {
        GameObject toDestroy = spawnedMultiBullets[0];

        if (toDestroy)
        {
            toDestroy.GetComponent<NetworkObject>().Despawn();
            spawnedMultiBullets.Remove(toDestroy);
            Destroy(toDestroy);
        }

    }


    //public void IncreaseFireRate(float addedFireRate)
    //{
    //    fireRateMultiplier += addedFireRate;
    //}

    [ClientRpc]
    public void FireRateIncreaseClientRpc(float addedFireRate)
    {
        fireRateMultiplier += addedFireRate;
    }

    //[ClientRpc]
    //public void ActivateMagnetBoolClientRpc()
    //{
    //    isMagnetPowerUpActive = true;
    //}

    //[ClientRpc]
    //public void ActivateMagnetTriggerBoolClientRpc()
    //{
    //    hasMagnetTriggered = true;
    //}

    //[ClientRpc]
    //private void DisableMagnetBoolClientRpc()
    //{
    //    isMagnetPowerUpActive = false;
    //}

    //[ClientRpc]
    //public void ActivateTripleBoolClientRpc()
    //{
    //    isTriplePowerUpActive.Value = true;
    //}

    [ServerRpc]
    private void CreateBulletNoiseServerRpc()
    {
        GameObject bullet = Instantiate(bulletNoise, transform.position + new Vector3(0, 2, 0), Quaternion.identity);

        if (isTriplePowerUpActive.Value)
        {
            bullet.GetComponent<MultiRandomPitch>().isTripleBulletActive = true;
            bullet.GetComponent<NetworkObject>().Spawn();
        }

        else
        {
            bullet.GetComponent<NetworkObject>().Spawn();
        }

    }

    [ServerRpc]
    private void UpdateMagnetUIServerRpc()
    {
        magnetPowerBar.GetComponent<MultiMagnetPowerBar>().UpdateMagnetBarClientRpc(originalMagnetPowerUpTime.Value, magnetPowerUpTime.Value);
    }

    [ServerRpc]
    private void UpdateTripleUIServerRpc()
    {
        triplePowerBar.GetComponent<MultiTripleBulletPowerBar>().UpdateTripleBarClientRpc(originalTriplePowerUpTime.Value, triplePowerUpTime.Value);
    }

    //[ServerRpc]
    //private void UpdateMagnetVariablesServerRpc()
    //{

    //    if (!IsOwner) return;

    //    magnetPowerUpTime -= Time.deltaTime;

    //    if (magnetPowerUpTime <= 0f)
    //    {
    //        magnetPowerUpTime = 0f;
    //        isMagnetPowerUpActive = false;
    //    }

    //}
    
    //[ServerRpc]
    //private void UpdateMagnetTriggerServerRpc()
    //{
    //    magnetPowerUpTime = originalMagnetPowerUpTime;
    //    hasMagnetTriggered = false;
    //}

    //[ServerRpc]
    //private void UpdateMagnetStuffServerRpc()
    //{

    //    UpdateMagnetStuffClientRpc();

    //}

    //[ClientRpc]
    //private void UpdateMagnetStuffClientRpc()
    //{

    //    if (hasMagnetTriggered)
    //    {
    //        magnetPowerUpTime = originalMagnetPowerUpTime;
    //        hasMagnetTriggered = false;
    //    }

    //    if (isMagnetPowerUpActive)
    //    {

    //        magnetPowerUpTime -= Time.deltaTime;

    //        if (magnetPowerUpTime <= 0f)
    //        {
    //            magnetPowerUpTime = 0f;
    //            isMagnetPowerUpActive = false;
    //        }

    //    }
    //}
}
