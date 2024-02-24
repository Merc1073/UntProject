using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using Unity.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager;

public class MultiMainPlayer : NetworkBehaviour
{

    [Header("Rapid Fire Text UI")]

    public GameObject RapidFireUICanvas;

    [SerializeField] private Text fireRateText;
    [SerializeField] private Text enemyRespawnText;
    [SerializeField] private Text enemyCountText;
    [SerializeField] private Text timerText;
    [SerializeField] private Text enemiesDefeatedText;
    [SerializeField] private Text orbsCollectedText;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text totalScoreText;


    [Header("Everything Else")]
    public Vector3 tranDif;

    public LayerMask groundMask;

    public float forceMultiplier;

    public NetworkVariable<int> coinCount = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> playerScore = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> enemyScore = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> playerScoreMultiplier = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> totalPlayerScore = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    //public string newEnemyTimerRounded;
    //public int enemyCounter;
    public NetworkVariable<FixedString128Bytes> newEnemyTimerRounded = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> enemyCounter = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<int> rapidTimer = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public AudioListener audioListener;

    //public bool isAlive = true;
    public NetworkVariable<bool> isAlive = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    //public bool hasPlayerDied = false;
    public NetworkVariable<bool> hasPlayerDied = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public bool particOnce = true;
    //public bool canMove = true;
    public NetworkVariable<bool> canMove = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public bool hasFoundGameScript = false;

    public GameObject particles;
    public MeshRenderer mesh;

    public Transform canvasTransform1;
    public Transform canvasTransform2;
    public Transform canvasTransform3;

    MultiGameScript multiGameScript;
    public GameObject multiBulletPoint;

    AudioSource src;
    public AudioClip gameOverNoise;

    private HealthBar playerHealthBar, magnetPowerBar, tripleBulletPowerBar;

    //private FollowMouse aimReticle;
    //private BulletPoint bulletReticle;

    Rigidbody rb;


    void Start()
    {

        if(!GetComponentInParent<NetworkObject>().IsSpawned)
        {
            GetComponentInParent<NetworkObject>().Spawn();
        }

        rb = GetComponent<Rigidbody>();

        playerHealthBar = GetComponentInChildren<HealthBar>();
        magnetPowerBar = GetComponentInChildren<HealthBar>();
        tripleBulletPowerBar = GetComponentInChildren<HealthBar>();

        //aimReticle = FindObjectOfType<FollowMouse>();
        //bulletReticle = FindObjectOfType<BulletPoint>();
        //multiGameScript = FindObjectOfType<MultiGameScript>();

        src = FindObjectOfType<AudioSource>();

        GetComponent<AudioListener>().enabled = false;

        RapidFireUICanvas.SetActive(false);

        if (!IsLocalPlayer) return;
        
        GetComponent<AudioListener>().enabled = true;

    }

    public override void OnNetworkSpawn()
    {
        UpdatePlayerVariablesServerRpc();

        if (!IsOwner) return;

        UpdatePositionServerRpc();
    }

    void Update()
    {

        //if(!GetComponent<AudioListener>().isActiveAndEnabled)
        //{
        //    GetComponent<AudioListener>().enabled = true;
        //}

        //if(SceneManager.GetActiveScene().name == "Multi Main Menu")
        //{
        //    RapidFireUICanvas.SetActive(false);
        //    coinCount.Value = 0;
        //    playerScore.Value = 0;
        //    playerScoreMultiplier.Value = 0;
        //}

        if(!multiGameScript && !hasFoundGameScript)
        {
            multiGameScript = FindObjectOfType<MultiGameScript>();
            //multiGameScript.GetComponent<MultiGameScript>().enabled = true;
            hasFoundGameScript = true;
        }


        if (IsLocalPlayer && multiGameScript.hasRapidFireModeStarted)
        {
            Debug.Log("works");
            RapidFireUICanvas.SetActive(true);
            fireRateText.text = "Fire Rate: " + multiBulletPoint.GetComponent<MultiBulletPoint>().roundsPerSecond.ToString() + " / Second";
            enemyRespawnText.text = "Enemy spawns every " + newEnemyTimerRounded.Value + " Seconds";
            enemyCountText.text = "Total Enemies in arena: " + enemyCounter.Value.ToString();
            timerText.text = "Time Elapsed: " + rapidTimer.Value.ToString();
            orbsCollectedText.text = "Orbs Collected: " + coinCount.Value.ToString();
            playerScoreText.text = "Your score: " + playerScore.Value.ToString();
            totalScoreText.text = "Total Score: " + totalPlayerScore.Value;

        }

        if (IsLocalPlayer && SceneManager.GetActiveScene().name == "Multi Main Menu")
        {
            RapidFireUICanvas.SetActive(false);
        }


        if (!IsOwner) return;

        UpdateFinalScoreServerRpc(totalPlayerScore.Value);

        audioListener.transform.position = transform.position;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (transform.position.y > 1.0f)
        {
            transform.position += new Vector3(0, -0.1f, 0) * Time.deltaTime * 100;
        }

        if(transform.position.y < -2.0f)
        {
            transform.position += new Vector3(0, 5f, 0);
        }

        if (GetComponentInParent<MultiHealthState>().HealthPoint.Value <= 0f && !hasPlayerDied.Value && isAlive.Value)
        {
            //if (!IsOwner) return;

            PlayerDiedPart1ServerRpc();

            //multiGameScript.GetComponent<MultiPlayerCount>().playerCount.Value--;
            //UpdatePlayerCountServerRpc();

            CreateParticlesServerRpc();

            //Debug.Log("Executed");
        }

        if(GetComponentInParent<MultiHealthState>().HealthPoint.Value < 0f)
        {
            UpdatePlayerHealthServerRpc(0f);
        }

        //else
        //{
        //    GetComponentInParent<MultiHealthState>().HealthPoint.Value = 5f;
        //    isAlive.Value = true;
        //    canMove = true;
        //    hasPlayerDied = false;
        //}


        if (canMove.Value == true)
        {
            rb.AddForce(movement * forceMultiplier * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {
        
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
    //    if (other.GetComponent<MultiBullet>() && GetComponentInParent<NetworkObject>().OwnerClientId != other.GetComponent<NetworkObject>().OwnerClientId)
    //    {
    //        GetComponentInParent<MultiHealthState>().HealthPoint.Value -= 1f;
    //        Debug.Log(GetComponentInParent<MultiHealthState>().HealthPoint.Value);

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

    [ServerRpc(RequireOwnership = false)]
    public void UpdateScoreServerRpc()
    {

        //if (!IsOwner) return;

        coinCount.Value += 1;
        playerScoreMultiplier.Value += 1;

        playerScore.Value = coinCount.Value * playerScoreMultiplier.Value + enemyScore.Value;

        //UpdatePlayerScoreClientRpc(playerScore.Value);

    }

    //[ServerRpc]
    //public void AddCoinsToPlayerClientRpc()
    //{

    //    //if (!IsOwner) return;

    //    coinCount.Value += 1;
    //    playerScoreMultiplier.Value += 1;

    //    playerScore.Value = coinCount.Value * playerScoreMultiplier.Value;

    //    UpdatePlayerScoreClientRpc(playerScore.Value);

    //    //multiGameScript = FindObjectOfType<MultiGameScript>();
    //    //multiGameScript.UpdateScoreMultiplierServerRpc(1f);
    //    //multiGameScript.coinCount += coins;

    //    //Debug.Log(multiGameScript.scoreMultiplier.Value);

    //}

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

    [ServerRpc(RequireOwnership = false)]
    private void UpdateFinalScoreServerRpc(float scoreValue)
    {
        totalPlayerScore.Value = scoreValue;
    }

    //[ClientRpc]
    //private void UpdatePlayerScoreClientRpc(float newScore)
    //{

    //    playerScore.Value = newScore;

    //    Debug.Log(newScore + " " + OwnerClientId);
    //}

    [ServerRpc(RequireOwnership = false)]
    private void CreateParticlesServerRpc()
    {
        GameObject deathParticle = Instantiate(particles, transform.position, Quaternion.identity);
        deathParticle.GetComponent<NetworkObject>().Spawn();
    }

    //[ServerRpc(RequireOwnership = false)]
    //private void UpdatePlayerCountServerRpc()
    //{
    //    multiGameScript.GetComponent<MultiPlayerCount>().playerCount.Value--;
    //}

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePlayerVariablesServerRpc()
    {
        isAlive.Value = true;
        hasPlayerDied.Value = false;
        canMove.Value = true;
        enemyScore.Value = 0f;
        totalPlayerScore.Value = 0f;
    }

    [ServerRpc(RequireOwnership = false)]
    private void PlayerDiedPart1ServerRpc()
    {
        multiBulletPoint.GetComponent<MultiBulletPoint>().canFire.Value = false;
        isAlive.Value = false;
        canMove.Value = false;
        hasPlayerDied.Value = true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdatePlayerHealthServerRpc(float healthChange)
    {
        GetComponentInParent<MultiHealthState>().HealthPoint.Value = healthChange;
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateEnemyScoreServerRpc(float addedScore)
    {
        enemyScore.Value += addedScore;
    }

    //[ServerRpc]
    //private void PlayerDiedPart2ServerRpc()
    //{
    //    hasPlayerDied.Value = true;
    //}

}
