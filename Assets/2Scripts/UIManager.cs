using DilmerGames.Core.Singletons;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    //[SerializeField]
    //private Button startServerButton;

    [SerializeField] private GameObject player;

    [SerializeField]
    private Button startHostButton;

    [SerializeField]
    private Button startClientButton;

    //[SerializeField]
    //private TextMeshProUGUI playersInGameText;

    [SerializeField]
    private TMP_InputField joinCodeInput;

    //[SerializeField]
    //private Button executePhysicsButton;

    //private bool hasServerStarted;

    private void Awake()
    {
        Cursor.visible = true;
    }

    void Update()
    {
        //playersInGameText.text = $"Players in game: {PlayersManager.Instance.PlayersInGame}";
    }

    void Start()
    {
        //// START SERVER
        //startServerButton?.onClick.AddListener(() =>
        //{
        //    if (NetworkManager.Singleton.StartServer())
        //        Logger.Instance.LogInfo("Server started...");
        //    else
        //        Logger.Instance.LogInfo("Unable to start server...");
        //});

        player = GameObject.FindGameObjectWithTag("MasterSinglePlayer");

        // START HOST
        startHostButton?.onClick.AddListener(async () =>
        {
            // this allows the UnityMultiplayer and UnityMultiplayerRelay scene to work with and without
            // relay features - if the Unity transport is found and is relay protocol then we redirect all the 
            // traffic through the relay, else it just uses a LAN type (UNET) communication.
            if (RelayManager.Instance.IsRelayEnabled)
            {
                await RelayManager.Instance.SetupRelay();
            }

            NetworkManager.Singleton.StartHost();
            
            if (player)
            {
                Destroy(player.gameObject);
            }

            SceneManager.LoadScene("Multi Main Menu");

            Debug.Log("Host started...");


            
            //Logger.Instance.LogInfo("Host started...");
            //else
                //Debug.Log("Unable to start host...");
        });

        // START CLIENT
        startClientButton?.onClick.AddListener(async () =>
        {
            //RelayManager.Instance.SetRelayServerData();
            if (RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCodeInput.text))
            {
                await RelayManager.Instance.JoinRelay(joinCodeInput.text);
            }

            NetworkManager.Singleton.StartClient();

            if (player)
            {
                Destroy(player.gameObject);
            }

        });

        // STATUS TYPE CALLBACKS
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            //Logger.Instance.LogInfo($"{id} just connected...");
            Debug.Log($"{id} just connected...");
        };

        //NetworkManager.Singleton.OnServerStarted += () =>
        //{
        //    hasServerStarted = true;
        //};

        //executePhysicsButton.onClick.AddListener(() => 
        //{
        //    if (!hasServerStarted)
        //    {
        //        Logger.Instance.LogWarning("Server has not started...");
        //        return;
        //    }
        //    SpawnerControl.Instance.SpawnObjects();
        //});
    }
}
