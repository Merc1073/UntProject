using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class MultiMainPlayer : NetworkBehaviour
{
    

    public Vector3 tranDif;

    public LayerMask groundMask;

    public float forceMultiplier;

    public NetworkVariable<int> coinCount = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> playerScore = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<int> playerScoreMultiplier = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    public AudioListener audioListener;
    //public NetworkVariable<float> currentHealth = new NetworkVariable<float>();
    //public NetworkVariable<float> maxHealth = new NetworkVariable<float>();
    //public float currentHealth;
    //public float maxHealth;

   // public int coinCount;

    public bool particOnce = true;
    public bool canMove = true;
    public bool hasFoundGameScript = false;

    public ParticleSystem particles;
    public MeshRenderer mesh;

    public Transform canvasTransform1;
    public Transform canvasTransform2;
    public Transform canvasTransform3;

    //MultiGameScript multiGameScript;
    public GameObject multiBulletPoint;

    AudioSource src;
    public AudioClip gameOverNoise;

    private HealthBar playerHealthBar, magnetPowerBar, tripleBulletPowerBar;

    private FollowMouse aimReticle;
    private BulletPoint bulletReticle;

    Rigidbody rb;

    void Start()
    {



        rb = GetComponent<Rigidbody>();

        playerHealthBar = GetComponentInChildren<HealthBar>();
        magnetPowerBar = GetComponentInChildren<HealthBar>();
        tripleBulletPowerBar = GetComponentInChildren<HealthBar>();

        aimReticle = FindObjectOfType<FollowMouse>();
        bulletReticle = FindObjectOfType<BulletPoint>();
        //multiGameScript = FindObjectOfType<MultiGameScript>();

        src = FindObjectOfType<AudioSource>();

        GetComponent<AudioListener>().enabled = false;

        if (!IsLocalPlayer) return;
        
        GetComponent<AudioListener>().enabled = true;
        

        //currentHealth = maxHealth;

        //if (playerHealthBar)
        //{
        //    playerHealthBar.UpdateHealthBar(maxHealth.Value, currentHealth.Value);
        //}

    }

    public override void OnNetworkSpawn()
    {
        //currentHealth.Value = 5f;
        //maxHealth.Value = 5f;

        if (!IsOwner) return;

        //multiGameScript = FindObjectOfType<MultiGameScript>();
        UpdatePositionServerRpc();
    }

    void Update()
    {

        if (!IsOwner) return;

        //Debug.Log(playerScore.Value + " " + OwnerClientId);
        //Debug.Log(hasFoundGameScript);

        //if (!multiGameScript && !hasFoundGameScript)
        //{
        //    multiGameScript = FindObjectOfType<MultiGameScript>();
        //    hasFoundGameScript = true;
        //}

        //if (multiGameScript)
        //{
        //    Debug.Log(multiGameScript.scoreMultiplier.Value);
        //}

        

        audioListener.transform.position = transform.position;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (transform.position.y > 1.0f)
        {
            transform.position += new Vector3(0, -0.1f, 0) * Time.deltaTime * 100;
        }

        //if (transform.position.y < 0.9f)
        //{
        //    transform.position += new Vector3(0, +0.1f, 0) * Time.deltaTime * 100;
        //}

        //if (currentHealth.Value < 0f)
        //{
        //    currentHealth.Value = 0;
        //}

        if (GetComponentInParent<MultiHealthState>().HealthPoint.Value <= 0f)
        {
            //if(!IsOwner) return;
            DeactivatePlayerServerRpc();
        }

        //if(Input.GetMouseButton(0))
        //{

        //    RaycastHit hit;

        //    if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, groundMask))
        //    {

        //        Vector3 directionToMouse = transform.position - hit.point;
        //        directionToMouse = directionToMouse.normalized * forceMultiplier;

        //        //clone = Instantiate(clone, hit.point + tranDif, rotation);
        //        rb.AddForce(-directionToMouse + tranDif * Time.deltaTime);
        //    }
        //}

        if (canMove == true)
        {
            rb.AddForce(movement * forceMultiplier * Time.deltaTime);
        }

        //if (!IsServer) return;
        if(Input.GetKeyDown(KeyCode.U))
        {
            GetComponentInParent<MultiHealthState>().HealthPoint.Value -= 1f;
            Debug.Log(GetComponentInParent<MultiHealthState>().HealthPoint.Value);
        }

    }

    private void LateUpdate()
    {
        canvasTransform1.position = transform.position + new Vector3(0, 4, 4);
        canvasTransform1.rotation = Quaternion.Euler(90, 0, 0);

        canvasTransform2.position = transform.position + new Vector3(0, 4, 2);
        canvasTransform2.rotation = Quaternion.Euler(90, 0, 0);

        canvasTransform3.position = transform.position + new Vector3(0, 4, -2);
        canvasTransform3.rotation = Quaternion.Euler(90, 0, 0);
        //canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!IsServer) return;
    //    if(other.GetComponent<MultiBullet>()/* && GetComponentInParent<NetworkObject>().OwnerClientId != other.GetComponent<NetworkObject>().OwnerClientId*/)
    //    {
    //        //GetComponentInParent<MultiHealthState>().HealthPoint.Value -= 1f;
    //        //Debug.Log(GetComponentInParent<MultiHealthState>().HealthPoint.Value);

    //        other.GetComponent<MultiBullet>().CreateParticlesServerRpc();
    //        other.GetComponent<MultiBullet>().GetComponent<NetworkObject>().Despawn();
    //        Destroy(other.GetComponent<MultiBullet>().gameObject);
    //    }
    //}


    //public void DecreasePlayerHealth(NetworkVariable<float> health)
    //{
    //    currentHealth.Value -= health;

    //    if (playerHealthBar)
    //    {
    //        playerHealthBar.UpdateHealthBar(maxHealth, currentHealth);
    //    }
    //}

    [ClientRpc]
    public void AddCoinsToPlayerClientRpc()
    {

        //if (!IsOwner) return;

        coinCount.Value += 1;
        playerScoreMultiplier.Value += 1;

        playerScore.Value = coinCount.Value * playerScoreMultiplier.Value;

        UpdatePlayerScoreClientRpc(playerScore.Value);

        //multiGameScript = FindObjectOfType<MultiGameScript>();
        //multiGameScript.UpdateScoreMultiplierServerRpc(1f);
        //multiGameScript.coinCount += coins;

        //Debug.Log(multiGameScript.scoreMultiplier.Value);

    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePositionServerRpc()
    {
        transform.position = new Vector3(0, 1, -10);
    }

    [ServerRpc]
    private void DeactivatePlayerServerRpc()
    {
        transform.parent.gameObject.SetActive(false);
    }

    [ClientRpc]
    private void UpdatePlayerScoreClientRpc(float newScore)
    {
        //if (!IsOwner) return;

        playerScore.Value = newScore;

        Debug.Log(newScore + " " + OwnerClientId);
    }

}
