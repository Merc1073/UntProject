using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;

public class MultiGameScript : NetworkBehaviour
{

    private static MultiGameScript instance;

    public static MultiGameScript Instance
    {
        get { return instance; }
    }

    [Header("Game Objects")]
    //public GameObject Player;
    public GameObject Enemy;
    public GameObject Coin;
    //public GameObject BulletPoint;
    //public GameObject Reticle;

    public GameObject MagnetPowerUp;
    public GameObject TripleBulletPowerUp;

    private GameObject mainPlayer;
    //private BulletPoint bulletReticle;

    //public FadeTransition fader;

    [Header("Special Booleans")]

    //public bool menuGameModeRapidFire = false;
    //public bool menuGameModeGrowth = false;

    public bool isMainPlayerFound = false;
    public bool networkSpawned = false;

    public bool isGameModeRapidFire = false;
    public bool isGameModeGrowing = false;

    public bool hasRapidFireModeStarted = false;
    public bool isCurrentSceneRapidFireMode = false;

    //public bool isMagnetPowerUpActive = false;
    //public bool isTripleBulletPowerUpActive = false;

    public bool hasGrowingModeStarted = false;

    public bool hasRapidFireSceneBeenLoaded = false;
    public bool hasMenuSceneBeenLoaded = false;

    //public bool hasRapidFireEnded = false;

    [Header("Texts")]
    [SerializeField] Text fireRateText;
    [SerializeField] Text respawnTimerText;
    [SerializeField] Text enemyCountText;
    [SerializeField] Text timerText;
    [SerializeField] Text enemiesKilled;
    [SerializeField] Text coinsCollected;
    [SerializeField] Text totalScore;
    [SerializeField] Text magnetText;
    [SerializeField] Text tripleBulletText;

    [Header("Positions")]
    public Vector3 playerMenuSpawn;
    public Vector3 playerRapidFireSpawn;

    public Vector3 enemySpawn;
    public Vector3 tranDif;

    public Vector3 bulletPointSpawn;
    public Vector3 reticlePointSpawn;

    public Vector3 magnetPowerUpSpawn;
    public Vector3 tripBulletPowerUpSpawn;

    [Header("Clock Timer")]
    [SerializeField] float totalTime;
    [SerializeField] int seconds;
    //[SerializeField] int respawnSeconds;
    [SerializeField] float timeToMainMenu;

    [Header("Powerups Respawn Timers")]
    [SerializeField] float originalTimerSpawnMagnetPowerUp;
    [SerializeField] float magnetRespawnTimer;

    [SerializeField] float originalTimerSpawnTripleBulletPowerUp;
    [SerializeField] float tripleBulletRespawnTimer;

    [Header("Powerups Lasting Timers")]
    [SerializeField] float magnetPowerUpTime;
    [SerializeField] float originalMagnetPowerUpTime;

    [SerializeField] float tripleBulletPowerUpTime;
    [SerializeField] float originalTripleBulletPowerUpTime;

    [Header("Enemy Respawn Timers")]
    public float enemyRespawnTimer;
    public float newEnemyTimer;

    [Header("Enemy Variables")]
    public float increaseInGlobalEnemyForceMultiplier;
    public float globalEnemyForceMultiplier;

    [Header("Coins and Score")]
    //public float coinCount;
    //public NetworkVariable<int> coinCount = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public float scoreCount;
    //public float scoreMultiplier;
    //public float addedEnemyScore;

    //public NetworkVariable<float> scoreCount = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<float> scoreMultiplier = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    //public NetworkVariable<float> addedEnemyScore = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public float initialScore = 0f;
    public NetworkVariable<float> totalFinalScore = new(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);


    [Header("Decimal Places")]
    public int genericDecimalPlaces;
    public int scoreDecimalPlaces;
    public int newEnemyDecimalPlaces;

    [Header("Enemy Timer Stages")]
    [SerializeField] float stage1;
    [SerializeField] float stage2;
    [SerializeField] float stage3;
    [SerializeField] float stage4;
    [SerializeField] float stage5;
    [SerializeField] float stage6;

    [Header("Other Variables")]
    [SerializeField] float enemySpawnRange;
    [SerializeField] float magnetSpawnRange;
    [SerializeField] float tripleBulletSpawnRange;
    [SerializeField] float invincibilitySpawnRange;

    [Header("Gamemode Booleans")]
    public bool skipTutorial = false;

    [Header("Player Booleans")]
    //public int playerCount;
    //public int originalPlayerCount;
    public bool isPlayerInvincible = false;

    [Header("Enemy Booleans")]
    public int enemyCount;
    public bool enemyFull = false;
    public bool keepReducingSpawnTimer = false;

    [Header("Spawn Booleans")]
    public bool canSpawnEnemies = false;
    public bool canSpawnMagnetPowerUp = false;
    public bool canSpawnTripleBulletPowerUp = false;

    [Header("Enemy variables")]
    //public int enemyCounter;
    public int enemyKillCounter;
    public int maxEnemies;

    [Header("Debug spawn in objects")]
    public bool spawnEnemyNow = false;
    public bool spawnCoinNow = false;
    public bool spawnMagnetPowerUpNow = false;
    public bool spawnTripleBulletPowerUpNow = false;

    public string scoreCountRounded;
    public string newEnemyTimerRounded;
    //public string magnetTimerRounded;
    //public string tripleBulletTimerRounded;

    //private void Start()
    //{
    //    GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
    //}

    private void Awake()
    {

        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }   

        instance = this;

        //DontDestroyOnLoad(this);
        DontDestroyOnLoad(gameObject);

        //if (SceneManager.GetActiveScene().name == "Rapid Fire")
        //{
        //    //isCurrentSceneRapidFireMode = true;

        //    Debug.Log("It's working");

        //    Instantiate(Player, playerSpawn, Quaternion.Euler(0, 0, 0));
        //    Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        //    Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));

        //    isGameModeRapidFire = true;

        //    stage1 = stage1ToSet;
        //    stage2 = stage2ToSet;
        //    stage3 = stage3ToSet;
        //    stage4 = stage4ToSet;
        //    stage5 = stage5ToSet;
        //    stage6 = stage6ToSet;

        //}

        //else
        //{
        //Instantiate(Player, playerMenuSpawn, Quaternion.Euler(0, 0, 0));
        //Instantiate(BulletPoint, bulletPointSpawn, Quaternion.Euler(0, 0, 0));
        //Instantiate(Reticle, reticlePointSpawn, Quaternion.Euler(0, 0, 0));
        //}

        if (hasRapidFireModeStarted == false)
        {
            fireRateText.gameObject.SetActive(false);
            respawnTimerText.gameObject.SetActive(false);
            enemyCountText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);
            enemiesKilled.gameObject.SetActive(false);
            coinsCollected.gameObject.SetActive(false);
            totalScore.gameObject.SetActive(false);
            magnetText.gameObject.SetActive(false);
            tripleBulletText.gameObject.SetActive(false);
        }



        //playerScript = FindObjectOfType<MultiMainPlayer>();
        //bulletReticle = FindObjectOfType<BulletPoint>();

        //if (!IsServer) return;

        if(!GetComponent<NetworkObject>().IsSpawned)
        {
            GetComponent<NetworkObject>().Spawn();
        }

    }

    //public override void OnNetworkSpawn()
    //{
    //    base.OnNetworkSpawn();
    //    scoreCount.Value = 0f;
    //    scoreMultiplier.Value = 1f;
    //    addedEnemyScore.Value = 0f;
    //}

    //public override void OnNetworkSpawn()
    //{
    //    if(IsServer)
    //    {

    //    }
    //    base.OnNetworkSpawn();
    //}

    void Update()
    {
        //playerCount = GetComponent<MultiPlayerCount>().playerCount.Value;
        enemyCount = GetComponent<MultiEnemyCount>().enemyCount;

        


        if (!mainPlayer && !isMainPlayerFound)
        {
            mainPlayer = GetComponent<MultiPlayerCount>().allPlayers[0];
            Debug.Log(mainPlayer);
            isMainPlayerFound = true;
        }

        //Debug.Log(scoreMultiplier.Value);

        //Debug.Log(mainPlayer.GetComponent<MultiMainPlayer>().playerScore.Value);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(SceneManager.GetActiveScene().name == "Multi Main Menu")
        {
            isGameModeRapidFire = false;
            hasRapidFireModeStarted = false;
            isCurrentSceneRapidFireMode = false;
            hasRapidFireSceneBeenLoaded = false;
            
        }

        if (SceneManager.GetActiveScene().name == "Multi Rapid Fire" && hasRapidFireSceneBeenLoaded == false)
        {
            isCurrentSceneRapidFireMode = true;
            hasMenuSceneBeenLoaded = false;

            //originalPlayerCount = GetComponent<MultiPlayerCount>().allPlayers.Count;

            //if (playerScript)
            //{
            //    playerScript = FindObjectOfType<MultiMainPlayer>();
            //}

            //if (bulletReticle)
            //{
            //    bulletReticle = FindObjectOfType<BulletPoint>();
            //}

            //if (mainPlayer)
            //{
            //    mainPlayer.transform.position = playerRapidFireSpawn;
            //}

            isGameModeRapidFire = true;

            //stage1 = stage1ToSet;
            //stage2 = stage2ToSet;
            //stage3 = stage3ToSet;
            //stage4 = stage4ToSet;
            //stage5 = stage5ToSet;
            //stage6 = stage6ToSet;

            hasRapidFireSceneBeenLoaded = true;

        }

        if (hasRapidFireModeStarted)
        {
            //fireRateText.gameObject.SetActive(true);
            //respawnTimerText.gameObject.SetActive(true);
            //enemyCountText.gameObject.SetActive(true);
            //timerText.gameObject.SetActive(true);
            //enemiesKilled.gameObject.SetActive(true);
            //coinsCollected.gameObject.SetActive(true);
            //totalScore.gameObject.SetActive(true);
            //magnetText.gameObject.SetActive(true);
            //tripleBulletText.gameObject.SetActive(true);

            keepReducingSpawnTimer = true;

            canSpawnEnemies = true;
            canSpawnMagnetPowerUp = true;
            canSpawnTripleBulletPowerUp = true;


        }

        if (newEnemyTimer <= 0f)
        {
            newEnemyTimer = 0f;
        }

        //totalFinalScore.Value = GetComponent<MultiPlayerCount>().allPlayers[0].GetComponent<MultiMainPlayer>().playerScore.Value + GetComponent<MultiPlayerCount>().allPlayers[1].GetComponent<MultiMainPlayer>().playerScore.Value;

        //scoreCount.Value = coinCount * scoreMultiplier.Value + addedEnemyScore.Value;

        scoreCountRounded = totalFinalScore.Value.ToString("F" + scoreDecimalPlaces);
        newEnemyTimerRounded = newEnemyTimer.ToString("F" + newEnemyDecimalPlaces);


        //if (isGameModeRapidFire == true)
        //{
        //    globalEnemyForceMultiplier += Time.deltaTime * increaseInGlobalEnemyForceMultiplier;
        //}

        if (spawnMagnetPowerUpNow == true)
        {
            DebugSpawnMagnetPowerUpServerRpc();
        }

        if (spawnTripleBulletPowerUpNow == true)
        {
            DebugSpawnTripleBulletPowerUpServerRpc();
        }

        if (spawnEnemyNow == true)
        {
            DebugSpawnEnemyServerRpc();
        }

        if (spawnCoinNow == true)
        {
            DebugSpawnCoinServerRpc();
        }


        if (hasRapidFireModeStarted && !AreAllPlayersDead())
        {

            globalEnemyForceMultiplier += Time.deltaTime * increaseInGlobalEnemyForceMultiplier;

            UpdateFinalScoreServerRpc();

            //Debug.Log(AreAllPlayersDead());


            if (AreAllPlayersDead())
            {
                Debug.Log("all players have died.");
            }

            //foreach(GameObject player in GetComponent<MultiPlayerCount>().allPlayers)
            //{
            //    Debug.Log("Player health: " + player.GetComponentInParent<MultiHealthState>().HealthPoint.Value.ToString());
            //}

            totalTime += Time.deltaTime;
            seconds = (int)(totalTime);

            //fireRateText.text = "Fire Rate: " + bulletReticle.roundsPerSecond.ToString() + " / Second";
            respawnTimerText.text = "Enemy spawns every " + newEnemyTimerRounded + " Seconds";
            enemyCountText.text = "Total Enemies in arena: " + GetComponent<MultiEnemyCount>().enemyCount.ToString();
            timerText.text = "Timer: " + seconds.ToString();
            enemiesKilled.text = "Enemies defeated: " + enemyKillCounter.ToString();
            //coinsCollected.text = "Orbs Collected: " + coinCount.ToString();
            totalScore.text = "Total Score: " + scoreCountRounded;

            for (int i = 0; i < GetComponent<MultiPlayerCount>().allPlayers.Count; i++)
            {
                GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().newEnemyTimerRounded.Value = newEnemyTimerRounded;
                GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().enemyCounter.Value = GetComponent<MultiEnemyCount>().enemyCount;
                GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().rapidTimer.Value = seconds;
            }

            //UpdatePlayerVariablesServerRpc();

            //magnetText.text = "Magnet Timer: " + magnetTimerRounded;
            //tripleBulletText.text = "Triple Bullet Timer: " + tripleBulletTimerRounded;

            if (canSpawnMagnetPowerUp == true)
            {
                magnetRespawnTimer -= Time.deltaTime;

                if (magnetRespawnTimer <= 0)
                {
                    SpawnMagnetPowerUpServerRpc();

                    magnetRespawnTimer = originalTimerSpawnMagnetPowerUp;
                }
            }

            if (canSpawnTripleBulletPowerUp == true)
            {
                tripleBulletRespawnTimer -= Time.deltaTime;

                if (tripleBulletRespawnTimer <= 0)
                {
                    SpawnTripleBulletPowerUpServerRpc();

                    tripleBulletRespawnTimer = originalTimerSpawnTripleBulletPowerUp;
                }
            }

            if (canSpawnEnemies == true)
            {

                if (GetComponent<MultiEnemyCount>().enemyCount >= maxEnemies)
                {
                    enemyFull = true;
                }

                if (GetComponent<MultiEnemyCount>().enemyCount < maxEnemies)
                {
                    enemyFull = false;
                }

                if (enemyFull == false)
                {

                    if (keepReducingSpawnTimer == true)
                    {

                        if (newEnemyTimer > 3.75f)
                        {
                            newEnemyTimer -= stage1 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            //respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemyServerRpc();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 2.5f)
                        {
                            newEnemyTimer -= stage2 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            //respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemyServerRpc();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 1.25f)
                        {
                            newEnemyTimer -= stage3 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            //respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemyServerRpc();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 0.625f)
                        {
                            newEnemyTimer -= stage4 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            //respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemyServerRpc();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else if (newEnemyTimer > 0.3125f)
                        {
                            newEnemyTimer -= stage5 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            //respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemyServerRpc();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }

                        else
                        {
                            newEnemyTimer -= stage6 * Time.deltaTime;
                            enemyRespawnTimer -= Time.deltaTime;

                            //respawnSeconds = (int)(enemyRespawnTimer % 60);

                            if (enemyRespawnTimer <= 0)
                            {
                                SpawnEnemyServerRpc();
                                enemyRespawnTimer = newEnemyTimer;
                            }
                        }
                    }

                    else
                    {
                        enemyRespawnTimer -= Time.deltaTime;

                        //respawnSeconds = (int)(enemyRespawnTimer % 60);

                        if (enemyRespawnTimer <= 0)
                        {
                            SpawnEnemyServerRpc();
                            enemyRespawnTimer = newEnemyTimer;
                        }
                    }
                }
            } 
        }


        if (AreAllPlayersDead())
        {

            Debug.Log("all players are dead.");

            keepReducingSpawnTimer = false;

            canSpawnEnemies = false;
            canSpawnMagnetPowerUp = false;
            canSpawnTripleBulletPowerUp = false;

            timeToMainMenu -= Time.deltaTime;

            if (timeToMainMenu <= 0f && !hasMenuSceneBeenLoaded)
            {
                //hasRapidFireEnded = true;
                isGameModeRapidFire = false;
                hasRapidFireModeStarted = false;
                isCurrentSceneRapidFireMode = false;
                hasRapidFireSceneBeenLoaded = false;

                totalTime = 0f;
                seconds = 0;

                magnetRespawnTimer = 5f;
                tripleBulletRespawnTimer = 10f;

                enemyRespawnTimer = 5f;
                newEnemyTimer = 5f;

                globalEnemyForceMultiplier = 500f;
                initialScore = 0f;

                enemyKillCounter = 0;

                DespawnAllEnemiesServerRpc();
                DespawnAllMagnetsServerRpc();
                DespawnAllTriplesServerRpc();
                DespawnAllCoinsServerRpc();

                //playerCount = originalPlayerCount;

                for (int i = 0; i < GetComponent<MultiPlayerCount>().allPlayers.Count; i++)
                {
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().GetComponentInParent<MultiHealthState>().HealthPoint.Value = 5f;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().canMove.Value = true;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().isAlive.Value = true;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().hasPlayerDied.Value = false;

                    //GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().SetCanvasFalseServerRpc();
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().coinCount.Value = 0;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().playerScore.Value = 0;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().playerScoreMultiplier.Value = 0;

                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().canFire.Value = true;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().fireRateMultiplier.Value = 1.5f;

                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().originalMagnetPowerUpTime.Value = 5f;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().magnetPowerUpTime.Value = 0f;

                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().originalTriplePowerUpTime.Value = 5f;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().triplePowerUpTime.Value = 0f;

                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().isMagnetPowerUpActive.Value = false;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().hasMagnetTriggered.Value = false;

                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().isTriplePowerUpActive.Value = false;
                    GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().multiBulletPoint.GetComponent<MultiBulletPoint>().hasTripleTriggered.Value = false;
                }

                NetworkManager.Singleton.SceneManager.LoadScene("Multi Main Menu", LoadSceneMode.Single);
                hasMenuSceneBeenLoaded = true;

                timeToMainMenu = 5f;

                return;
            }
        }

        //else
        //{
        //    Debug.Log("It's not working :/");
        //}



    }

    public bool AreAllPlayersDead()
    {

        foreach(GameObject player in GetComponent<MultiPlayerCount>().allPlayers)
        {
            if(player.GetComponentInParent<MultiHealthState>().HealthPoint.Value != 0)
            {
                return false;
            }
        }

        return true;
    }

    [ServerRpc]
    public void DebugSpawnEnemyServerRpc()
    {
        GameObject debugEnemy = Instantiate(Enemy, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        debugEnemy.GetComponent<NetworkObject>().Spawn();
        //enemyCounter++;
        spawnEnemyNow = false;
    }

    [ServerRpc]
    public void SpawnEnemyServerRpc()
    {
        enemySpawn = new Vector3(Random.Range(enemySpawnRange, -enemySpawnRange), 1, (Random.Range(enemySpawnRange, -enemySpawnRange)));
        GameObject normalEnemy = Instantiate(Enemy, enemySpawn + tranDif, Quaternion.Euler(0, 0, 0));
        normalEnemy.GetComponent<NetworkObject>().Spawn();
        //enemyCounter++;
    }

    [ServerRpc]
    private void DebugSpawnCoinServerRpc()
    {
        GameObject coin = Instantiate(Coin, new Vector3(0, 0, -40), Quaternion.Euler(0, 0, 0));
        coin.GetComponent<NetworkObject>().Spawn();
        spawnCoinNow = false;
    }

    public void ReduceEnemy()
    {
        //addedEnemyScore.Value += 50f;
        enemyKillCounter++;
        //GetComponent<MultiEnemyCount>().enemyCount--;
        //enemyCounter--;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnAllEnemiesServerRpc()
    {
        //GameObject[] enemiesToDestroy = GameObject.FindGameObjectsWithTag("MultiEnemy");

        //foreach(GameObject enemy in enemiesToDestroy)
        //{
        //    enemy.GetComponent<NetworkObject>().Despawn();
        //    Destroy(enemy);
        //}

        //for (int i = 0; i < GetComponent<MultiEnemyCount>().enemyCount; i++)
        //{
        //    if (GetComponent<MultiEnemyCount>().allEnemies[i] != null)
        //    {
        //        GetComponent<MultiEnemyCount>().allEnemies[i].GetComponentInParent<NetworkObject>().Despawn();
        //        Destroy(GetComponent<MultiEnemyCount>().allEnemies[i]);
        //        GetComponent<MultiEnemyCount>().enemyCount--;
        //    }
        //}

        foreach(GameObject enemy in GetComponent<MultiEnemyCount>().allEnemies)
        {
            enemy.GetComponentInParent<NetworkObject>().Despawn();
            Destroy(enemy);
            GetComponent<MultiEnemyCount>().allEnemies.Remove(enemy);
            //GetComponent<MultiEnemyCount>().enemyCount--;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnAllMagnetsServerRpc()
    {
        foreach (GameObject magnet in GetComponent<MultiMagnetCount>().allMagnets)
        {
            magnet.GetComponentInParent<NetworkObject>().Despawn();
            Destroy(magnet);
            GetComponent<MultiMagnetCount>().allMagnets.Remove(magnet);
            //GetComponent<MultiEnemyCount>().enemyCount--;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnAllTriplesServerRpc()
    {
        foreach (GameObject triple in GetComponent<MultiTripleCount>().allTriples)
        {
            triple.GetComponentInParent<NetworkObject>().Despawn();
            Destroy(triple);
            GetComponent<MultiTripleCount>().allTriples.Remove(triple);
            //GetComponent<MultiEnemyCount>().enemyCount--;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnAllCoinsServerRpc()
    {
        foreach (GameObject coin in GetComponent<MultiCoinCount>().allCoins)
        {
            coin.GetComponentInParent<NetworkObject>().Despawn();
            Destroy(coin);
            GetComponent<MultiCoinCount>().allCoins.Remove(coin);
            //GetComponent<MultiEnemyCount>().enemyCount--;
        }
    }

    private void UpdateFinalScore()
    {

        initialScore = 0f;
        
        for (int i = 0; i < GetComponent<MultiPlayerCount>().allPlayers.Count; i++)
        {
            if(GetComponent<MultiPlayerCount>().allPlayers[i])
            {
                initialScore += GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().playerScore.Value;
                totalFinalScore.Value = initialScore;
                //GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().totalPlayerScore.Value = initialScore;
            }
        }

        for(int i = 0; i < GetComponent<MultiPlayerCount>().allPlayers.Count; i++)
        {
            if(GetComponent<MultiPlayerCount>().allPlayers[i])
            {
                GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().totalPlayerScore.Value = initialScore;
            }
        }


    }

    [ServerRpc]
    private void UpdateFinalScoreServerRpc()
    {
        UpdateFinalScore();
    }

    //public void ActivateMagnetPowerUp()
    //{
    //    magnetPowerUpTime = originalMagnetPowerUpTime;
    //    isMagnetPowerUpActive = true;
    //}

    //public void ActivateTripleBulletPowerUp()
    //{
    //    tripleBulletPowerUpTime = originalTripleBulletPowerUpTime;
    //    isTripleBulletPowerUpActive = true;
    //}

    [ServerRpc]
    public void DebugSpawnMagnetPowerUpServerRpc()
    {
        GameObject magnet = Instantiate(MagnetPowerUp, new Vector3(0, 0, -40), Quaternion.Euler(0, 0, 0));
        magnet.GetComponent<NetworkObject>().Spawn();

        spawnMagnetPowerUpNow = false;
    }

    [ServerRpc]
    public void SpawnMagnetPowerUpServerRpc()
    {
        GameObject magnet = Instantiate(MagnetPowerUp, new Vector3(Random.Range(-magnetSpawnRange, magnetSpawnRange), 0, Random.Range(-magnetSpawnRange, magnetSpawnRange)), Quaternion.Euler(0, 0, 0));
        magnet.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    public void DebugSpawnTripleBulletPowerUpServerRpc()
    {
        GameObject triple = Instantiate(TripleBulletPowerUp, new Vector3(0, 0, -40), Quaternion.Euler(0, 0, 0));
        triple.GetComponent<NetworkObject>().Spawn();

        spawnTripleBulletPowerUpNow = false;
    }

    [ServerRpc]
    public void SpawnTripleBulletPowerUpServerRpc()
    {
        GameObject triple = Instantiate(TripleBulletPowerUp, new Vector3(Random.Range(-tripleBulletSpawnRange, tripleBulletSpawnRange), 0, Random.Range(-tripleBulletSpawnRange, tripleBulletSpawnRange)), Quaternion.Euler(0, 0, 0));
        triple.GetComponent<NetworkObject>().Spawn();
    }

    //[ServerRpc]
    //public void UpdateScoreMultiplierServerRpc(float scoreToIncrease)
    //{
    //    if(!IsOwner) return;

    //    scoreMultiplier.Value += scoreToIncrease;
    //    Debug.Log(scoreMultiplier.Value);
    //}

    //[ServerRpc]
    //private void UpdatePlayerVariablesServerRpc()
    //{
    //    UpdatePlayerVariablesClientRpc();
    //}

    //[ClientRpc]
    //private void UpdatePlayerVariablesClientRpc()
    //{
    //    for (int i = 0; i < GetComponent<MultiPlayerCount>().allPlayers.Count; i++)
    //    {
    //        GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().newEnemyTimerRounded = newEnemyTimerRounded;
    //        GetComponent<MultiPlayerCount>().allPlayers[i].GetComponent<MultiMainPlayer>().enemyCounter.Value = enemyCounter;
    //    }
    //}
}
