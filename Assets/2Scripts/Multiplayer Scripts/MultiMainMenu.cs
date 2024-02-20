using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiMainMenu : NetworkBehaviour
{
    Transform startButton;
    Transform gameModesButton;
    Transform quitButton;

    Transform yesButton;
    Transform noButton;
    Transform backButtonStart;

    Transform rapidFireButton;
    Transform growthButton;
    Transform backButtonGameModes;


    public Text tutorialText;

    public TMP_Text rapidFireText;
    public TMP_Text growthText;

    public GameObject particleEffect1;
    public GameObject particleEffect2;

    //MenuPlaySound soundPlay;

    public bool networkSpawned = false;

    public bool startRapidFire = false;
    public bool startGrowth = false;

    public bool gameModeIsRapidFire = true;
    public bool gameModeIsGrowth = false;

    public bool gameEnd = false;

    public bool startButtonActivated = false;
    public bool quitButtonActivated = false;

    private float timer;
    public float generalDistanceFromPlayer;
    public float PlayerCannotMoveForXSeconds;

    //public ParticleSystem startButtonParticle;
    //public ParticleSystem gameModesButtonParticle;
    //public ParticleSystem quitButtonParticle;

    GameObject player;
    MultiGameScript multiGameScript;

    public bool particOnce = true;

    FadeTransition sceneTransition;

    public float startDistanceToPlayer;
    public float gameModeDistanceToPlayer;
    public float quitDistanceToPlayer;

    public float yesDistanceToPlayer;
    public float noDistanceToPlayer;
    public float backDistanceToPlayer;

    public float rapidFireDistanceToPlayer;
    public float growthDistanceToPlayer;
    public float backGameModesDistanceToPlayer;

    void Start()
    {

        multiGameScript = FindObjectOfType<MultiGameScript>();
        sceneTransition = FindObjectOfType<FadeTransition>();

        startButton = transform.GetChild(0);
        gameModesButton = transform.GetChild(1);
        quitButton = transform.GetChild(2);

        yesButton = transform.GetChild(3);
        noButton = transform.GetChild(4);
        backButtonStart = transform.GetChild(5);

        rapidFireButton = transform.GetChild(6);
        growthButton = transform.GetChild(7);
        backButtonGameModes = transform.GetChild(8);

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        backButtonStart.gameObject.SetActive(false);

        rapidFireButton.gameObject.SetActive(false);
        growthButton.gameObject.SetActive(false);
        backButtonGameModes.gameObject.SetActive(false);

        tutorialText.gameObject.SetActive(false);

        rapidFireText.gameObject.SetActive(false);
        growthText.gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("MultiPlayer");

        //if (!IsServer) return;

        GetComponent<NetworkObject>().SpawnWithOwnership(clientId: 0);
        GetComponent<NetworkObject>().DestroyWithScene = true;

    }

    void Update()
    {

        if (gameModeIsRapidFire == true)
        {
            rapidFireText.gameObject.SetActive(true);
            growthText.gameObject.SetActive(false);
        }

        if (gameModeIsGrowth == true)
        {
            rapidFireText.gameObject.SetActive(false);
            growthText.gameObject.SetActive(true);
        }

        if (startButton && player)
        {
            startDistanceToPlayer = Vector3.Distance(startButton.transform.position, player.transform.position);
        }

        if (gameModesButton && player)
        {
            gameModeDistanceToPlayer = Vector3.Distance(gameModesButton.position, player.transform.position);
        }

        if (quitButton && player)
        {
            quitDistanceToPlayer = Vector3.Distance(quitButton.position, player.transform.position);
        }

        if (yesButton && player)
        {
            yesDistanceToPlayer = Vector3.Distance(yesButton.transform.position, player.transform.position);
        }

        if (noButton && player)
        {
            noDistanceToPlayer = Vector3.Distance(noButton.transform.position, player.transform.position);
        }

        if (backButtonStart && player)
        {
            backDistanceToPlayer = Vector3.Distance(backButtonStart.transform.position, player.transform.position);
        }

        if (rapidFireButton && player)
        {
            rapidFireDistanceToPlayer = Vector3.Distance(rapidFireButton.transform.position, player.transform.position);
        }

        if (growthButton && player)
        {
            growthDistanceToPlayer = Vector3.Distance(growthButton.transform.position, player.transform.position);
        }

        if (backButtonGameModes && player)
        {
            backGameModesDistanceToPlayer = Vector3.Distance(backButtonGameModes.transform.position, player.transform.position);
        }


        if (startButton.gameObject.activeSelf == true)
        {
            if (startDistanceToPlayer <= 2f && particOnce)
            {
                //startRapidFire = true;
                //StartCoroutine(sceneTransition.FadeOutTransition());



                CreateParticle1ServerRpc();
                //soundPlay.canPlaySelectOptionSound = true;
                //soundPlay.canPlayGameStart = true;

                ActivateObject(yesButton.gameObject);
                ActivateObject(noButton.gameObject);
                ActivateObject(backButtonStart.gameObject);
                ActivateObject(tutorialText.gameObject);

                DeactivateObject(startButton.gameObject);
                DeactivateObject(gameModesButton.gameObject);
                DeactivateObject(quitButton.gameObject);

            }
        }

        if (gameModesButton.gameObject.activeSelf == true)
        {
            if (gameModeDistanceToPlayer <= generalDistanceFromPlayer)
            {

                CreateParticle2ServerRpc();

                ActivateObject(rapidFireButton.gameObject);
                ActivateObject(growthButton.gameObject);
                ActivateObject(backButtonGameModes.gameObject);

                DeactivateObject(startButton.gameObject);
                DeactivateObject(gameModesButton.gameObject);
                DeactivateObject(quitButton.gameObject);
            }
        }

        if (quitButton.gameObject.activeSelf == true)
        {
            if (quitDistanceToPlayer <= generalDistanceFromPlayer)
            {
                gameEnd = true;
                StartCoroutine(sceneTransition.FadeOutTransition());


                CreateParticle3ServerRpc();

                DeactivateObject(startButton.gameObject);
                DeactivateObject(gameModesButton.gameObject);
                DeactivateObject(quitButton.gameObject);
            }
        }

        if (yesButton.gameObject.activeSelf == true)
        {
            if (yesDistanceToPlayer <= generalDistanceFromPlayer)
            {
                //startRapidFire = true;
                StartCoroutine(sceneTransition.FadeOutTransition());


                CreateParticle4ServerRpc();

                DeactivateObject(yesButton.gameObject);
                DeactivateObject(noButton.gameObject);
                DeactivateObject(backButtonStart.gameObject);
            }
        }

        if (noButton.gameObject.activeSelf == true)
        {
            if (noDistanceToPlayer <= generalDistanceFromPlayer)
            {
                //SkipTutorialServerRpc();
                multiGameScript.skipTutorial = true;
                startRapidFire = true;
                //StartCoroutine(sceneTransition.FadeOutTransition());
                //LoadThisSceneServerRpc();

                CreateParticle5ServerRpc();
                NetworkManager.SceneManager.LoadScene("Multi Rapid Fire", LoadSceneMode.Single);


                DeactivateObject(yesButton.gameObject);
                DeactivateObject(noButton.gameObject);
                DeactivateObject(backButtonStart.gameObject);
            }
        }

        if (backButtonStart.gameObject.activeSelf == true)
        {
            if (backDistanceToPlayer <= generalDistanceFromPlayer)
            {

                CreateParticle6ServerRpc();

                DeactivateObject(yesButton.gameObject);
                DeactivateObject(noButton.gameObject);
                DeactivateObject(backButtonStart.gameObject);
                DeactivateObject(tutorialText.gameObject);

                ActivateObject(startButton.gameObject);
                ActivateObject(gameModesButton.gameObject);
                ActivateObject(quitButton.gameObject);
            }
        }

        if (rapidFireButton.gameObject.activeSelf == true)
        {
            if (rapidFireDistanceToPlayer <= generalDistanceFromPlayer)
            {

                CreateParticle7ServerRpc();

                gameModeIsRapidFire = true;
                gameModeIsGrowth = false;

                player.transform.position = multiGameScript.playerMenuSpawn;
                StartCoroutine(NoPlayerMovement(PlayerCannotMoveForXSeconds));

                DeactivateObject(rapidFireButton.gameObject);
                DeactivateObject(growthButton.gameObject);
                DeactivateObject(backButtonGameModes.gameObject);

                ActivateObject(startButton.gameObject);
                ActivateObject(gameModesButton.gameObject);
                ActivateObject(quitButton.gameObject);
            }
        }

        if (growthButton.gameObject.activeSelf == true)
        {
            if (growthDistanceToPlayer <= generalDistanceFromPlayer)
            {

                CreateParticle8ServerRpc();

                gameModeIsRapidFire = false;
                gameModeIsGrowth = true;

                player.transform.position = multiGameScript.playerMenuSpawn;
                StartCoroutine(NoPlayerMovement(PlayerCannotMoveForXSeconds));

                DeactivateObject(rapidFireButton.gameObject);
                DeactivateObject(growthButton.gameObject);
                DeactivateObject(backButtonGameModes.gameObject);

                ActivateObject(startButton.gameObject);
                ActivateObject(gameModesButton.gameObject);
                ActivateObject(quitButton.gameObject);
            }
        }

        if (backButtonGameModes.gameObject.activeSelf == true)
        {
            if (backGameModesDistanceToPlayer <= generalDistanceFromPlayer)
            {

                CreateParticle9ServerRpc();

                DeactivateObject(rapidFireButton.gameObject);
                DeactivateObject(growthButton.gameObject);
                DeactivateObject(backButtonGameModes.gameObject);

                ActivateObject(startButton.gameObject);
                ActivateObject(gameModesButton.gameObject);
                ActivateObject(quitButton.gameObject);
            }
        }

    }

    private IEnumerator NoPlayerMovement(float duration)
    {
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;

            yield return null;
        }
    }

    //private IEnumerator EndGame()
    //{
    //    timer += Time.deltaTime;

    //    if (timer >= 1f)
    //    {
    //        Debug.Log("Game end");
    //        Application.Quit();
    //        yield return null;
    //    }

    //}

    //public void StartButtonParticlePlay()
    //{
    //    var em = startButtonParticle.emission;

    //    em.enabled = true;

    //    startButtonParticle.Play();

    //    particOnce = false;

    //    DeactivateObject(startButton.gameObject);

    //    //Destroy(mesh);
    //    //Invoke(nameof(DeactivateObject), 0);
    //}

    //public void CreateParticleEffect(GameObject particle, Vector3 position, Quaternion rotation)
    //{

    //    Instantiate(particle, position, rotation);

    //}

    public void DeactivateObject(GameObject objectToDeactivate)
    {
        objectToDeactivate.SetActive(false);
    }

    public void ActivateObject(GameObject objectToActivate)
    {
        objectToActivate.SetActive(true);
    }

    [ServerRpc]
    private void CreateParticle1ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect1, startButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle2ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect1, gameModesButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle3ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect2, quitButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle4ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect2, yesButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle5ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect2, noButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle6ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect1, backButtonStart.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle7ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect1, rapidFireButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle8ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect1, growthButton.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void CreateParticle9ServerRpc()
    {
        GameObject particle = Instantiate(particleEffect1, backButtonGameModes.position, Quaternion.identity);
        particle.GetComponent<NetworkObject>().Spawn();
    }


}
