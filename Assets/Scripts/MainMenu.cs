using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;
using TMPro;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{

    Transform player;
    CameraFollow camFollow;
    BulletPoint bulletPoint;

    Transform startButton;
    Transform gameModesButton;
    Transform quitButton;

    Transform yesButton;
    Transform noButton;
    Transform backButtonStart;

    Transform rapidFireButton;
    Transform growthButton;
    Transform backButtonGameModes;

    Transform startHostButton;
    Transform startClientButton;


    public Text tutorialText;

    public TMP_Text rapidFireText;
    public TMP_Text growthText;

    public GameObject particleEffect;

    MenuPlaySound soundPlay;

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

    //GameObject player;
    GameScript gameScript;

    public bool particOnce = true;
    public bool doesPlayerExist = false;

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

    public float startHostDistanceToPlayer;
    public float startClientDistanceToPlayer;

    void Start()
    {

        camFollow = FindObjectOfType<CameraFollow>();
        gameScript = FindObjectOfType<GameScript>();
        bulletPoint = FindObjectOfType<BulletPoint>();
        sceneTransition = FindObjectOfType<FadeTransition>();
        soundPlay = GetComponentInParent<MenuPlaySound>();
        
        startButton = transform.GetChild(0);
        gameModesButton = transform.GetChild(1);
        quitButton = transform.GetChild(2);

        yesButton = transform.GetChild(3);
        noButton = transform.GetChild(4);
        backButtonStart = transform.GetChild(5);

        rapidFireButton = transform.GetChild(6);
        growthButton = transform.GetChild(7);
        backButtonGameModes = transform.GetChild(8);

        startHostButton = transform.GetChild(9);
        startClientButton = transform.GetChild(10);

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        backButtonStart.gameObject.SetActive(false);

        rapidFireButton.gameObject.SetActive(false);
        growthButton.gameObject.SetActive(false);
        backButtonGameModes.gameObject.SetActive(false);

        tutorialText.gameObject.SetActive(false);

        rapidFireText.gameObject.SetActive(false);
        growthText.gameObject.SetActive(false);

        //player = gameScript.GetComponent<Detection>().targetPlayer;

    }

    void Update()
    {

        if(!player && !doesPlayerExist)
        {
            player = gameScript.GetComponent<Detection>().targetPlayer;
            doesPlayerExist = true;
        }

        if(player)
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

            if (startButton)
            {
                startDistanceToPlayer = Vector3.Distance(startButton.transform.position, player.transform.position);
            }

            if (gameModesButton)
            {
                gameModeDistanceToPlayer = Vector3.Distance(gameModesButton.position, player.transform.position);
            }

            if (quitButton)
            {
                quitDistanceToPlayer = Vector3.Distance(quitButton.position, player.transform.position);
            }

            if (yesButton)
            {
                yesDistanceToPlayer = Vector3.Distance(yesButton.transform.position, player.transform.position);
            }

            if (noButton)
            {
                noDistanceToPlayer = Vector3.Distance(noButton.transform.position, player.transform.position);
            }

            if (backButtonStart)
            {
                backDistanceToPlayer = Vector3.Distance(backButtonStart.transform.position, player.transform.position);
            }

            if (rapidFireButton)
            {
                rapidFireDistanceToPlayer = Vector3.Distance(rapidFireButton.transform.position, player.transform.position);
            }

            if (growthButton)
            {
                growthDistanceToPlayer = Vector3.Distance(growthButton.transform.position, player.transform.position);
            }

            if (backButtonGameModes)
            {
                backGameModesDistanceToPlayer = Vector3.Distance(backButtonGameModes.transform.position, player.transform.position);
            }

            if(startHostButton)
            {
                startHostDistanceToPlayer = Vector3.Distance(startHostButton.transform.position, player.transform.position);
            }

            if(startClientButton)
            {
                startClientDistanceToPlayer = Vector3.Distance(startClientButton.transform.position, player.transform.position);
            }





            if (startButton.gameObject.activeSelf == true)
            {
                if (startDistanceToPlayer <= 2f && particOnce)
                {
                    //startRapidFire = true;
                    //StartCoroutine(sceneTransition.FadeOutTransition());

                    CreateParticleEffect(particleEffect, startButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();
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
                    CreateParticleEffect(particleEffect, gameModesButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

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

                    CreateParticleEffect(particleEffect, quitButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayStartSound();

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

                    CreateParticleEffect(particleEffect, yesButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayStartSound();

                    DeactivateObject(yesButton.gameObject);
                    DeactivateObject(noButton.gameObject);
                    DeactivateObject(backButtonStart.gameObject);
                }
            }

            if (noButton.gameObject.activeSelf == true)
            {
                if (noDistanceToPlayer <= generalDistanceFromPlayer)
                {
                    gameScript.skipTutorial = true;
                    //startRapidFire = true;
                    StartCoroutine(sceneTransition.FadeOutTransition());

                    CreateParticleEffect(particleEffect, noButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayStartSound();

                    DeactivateObject(yesButton.gameObject);
                    DeactivateObject(noButton.gameObject);
                    DeactivateObject(backButtonStart.gameObject);
                }
            }

            if (backButtonStart.gameObject.activeSelf == true)
            {
                if (backDistanceToPlayer <= generalDistanceFromPlayer)
                {
                    CreateParticleEffect(particleEffect, backButtonStart.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

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
                    CreateParticleEffect(particleEffect, rapidFireButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

                    gameModeIsRapidFire = true;
                    gameModeIsGrowth = false;

                    player.transform.position = gameScript.playerMenuSpawn;
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
                    CreateParticleEffect(particleEffect, growthButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

                    gameModeIsRapidFire = false;
                    gameModeIsGrowth = true;

                    player.transform.position = gameScript.playerMenuSpawn;
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
                    CreateParticleEffect(particleEffect, backButtonGameModes.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

                    DeactivateObject(rapidFireButton.gameObject);
                    DeactivateObject(growthButton.gameObject);
                    DeactivateObject(backButtonGameModes.gameObject);

                    ActivateObject(startButton.gameObject);
                    ActivateObject(gameModesButton.gameObject);
                    ActivateObject(quitButton.gameObject);
                }
            }

            if(startHostButton.gameObject.activeSelf == true)
            {
                if(startHostDistanceToPlayer <= generalDistanceFromPlayer)
                {
                    CreateParticleEffect(particleEffect, startHostButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

                    Destroy(player.gameObject);

                    NetworkManager.Singleton.StartHost();

                    doesPlayerExist = false;
                    gameScript.doesPlayerExist = false;
                    camFollow.doesPlayerExist = false;
                    bulletPoint.doesPlayerExist = false;


                    DeactivateObject(startHostButton.gameObject);
                    DeactivateObject(startClientButton.gameObject);
                }
            }

            if(startClientButton.gameObject.activeSelf == true)
            {
                if(startClientDistanceToPlayer <= generalDistanceFromPlayer)
                {
                    CreateParticleEffect(particleEffect, startClientButton.transform.position, Quaternion.Euler(Vector3.zero));

                    soundPlay.PlayOptionSelectSound();

                    Destroy(player.gameObject);

                    NetworkManager.Singleton.StartClient();

                    doesPlayerExist = false;
                    gameScript.doesPlayerExist = false;
                    camFollow.doesPlayerExist = false;
                    bulletPoint.doesPlayerExist = false;

                    DeactivateObject(startHostButton.gameObject);
                    DeactivateObject(startClientButton.gameObject);
                }
            }

        }
        

    }

    private IEnumerator NoPlayerMovement(float duration)
    {
        float startTime = Time.time;

        while(Time.time - startTime < duration)
        {
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;

            yield return null;
        }
    }

    public void CreateParticleEffect(GameObject particle, Vector3 position, Quaternion rotation)
    {

        Instantiate(particle, position, rotation);

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

    public void DeactivateObject(GameObject objectToDeactivate)
    {
        objectToDeactivate.SetActive(false);
    }

    public void ActivateObject(GameObject objectToActivate)
    {
        objectToActivate.SetActive(true);
    }

}
