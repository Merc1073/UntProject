using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class MainMenu : MonoBehaviour
{

    Transform startButton;
    Transform gameModesButton;
    Transform quitButton;

    Transform yesButton;
    Transform noButton;
    Transform backButton;

    public Text tutorialText;

    public GameObject particleEffect;

    MenuPlaySound soundPlay;

    public bool startRapidFire = false;
    public bool startGrowth = false;

    public bool gameEnd = false;

    public bool startButtonActivated = false;
    public bool quitButtonActivated = false;

    private float timer;

    //public ParticleSystem startButtonParticle;
    //public ParticleSystem gameModesButtonParticle;
    //public ParticleSystem quitButtonParticle;

    GameObject player;
    GameScript gameScript;

    public bool particOnce = true;

    FadeTransition sceneTransition;

    public float startDistanceToPlayer;
    public float gameModeDistanceToPlayer;
    public float quitDistanceToPlayer;

    public float yesDistanceToPlayer;
    public float noDistanceToPlayer;
    public float backDistanceToPlayer;

    void Start()
    {

        gameScript = FindObjectOfType<GameScript>();
        sceneTransition = FindObjectOfType<FadeTransition>();
        soundPlay = GetComponentInParent<MenuPlaySound>();
        
        startButton = transform.GetChild(0);
        gameModesButton = transform.GetChild(1);
        quitButton = transform.GetChild(2);

        yesButton = transform.GetChild(3);
        noButton = transform.GetChild(4);
        backButton = transform.GetChild(5);

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);

        tutorialText.gameObject.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Update()
    {

        if(startButton)
        {
            startDistanceToPlayer = Vector3.Distance(startButton.transform.position, player.transform.position);
        }

        if(gameModesButton) 
        {
            gameModeDistanceToPlayer = Vector3.Distance(gameModesButton.position, player.transform.position);
        }

        if(quitButton)
        {
            quitDistanceToPlayer = Vector3.Distance(quitButton.position, player.transform.position);
        }

        if(yesButton)
        {
            yesDistanceToPlayer = Vector3.Distance(yesButton.transform.position, player.transform.position);
        }

        if(noButton)
        {
            noDistanceToPlayer = Vector3.Distance(noButton.transform.position, player.transform.position);
        }

        if(backButton)
        {
            backDistanceToPlayer = Vector3.Distance(backButton.transform.position, player.transform.position);
        }

        if(startButton.gameObject.activeSelf == true)
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
                ActivateObject(backButton.gameObject);
                ActivateObject(tutorialText.gameObject);

                DeactivateObject(startButton.gameObject);
                DeactivateObject(gameModesButton.gameObject);
                DeactivateObject(quitButton.gameObject);
            }
        }
        
        if(quitButton.gameObject.activeSelf == true)
        {
            if (quitDistanceToPlayer <= 2f)
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
        
        if(yesButton.gameObject.activeSelf == true)
        {
            if(yesDistanceToPlayer <= 2f)
            {
                startRapidFire = true;
                StartCoroutine(sceneTransition.FadeOutTransition());

                CreateParticleEffect(particleEffect, yesButton.transform.position, Quaternion.Euler(Vector3.zero));

                soundPlay.PlayStartSound();

                DeactivateObject(yesButton.gameObject);
                DeactivateObject(noButton.gameObject);
                DeactivateObject(backButton.gameObject);
            }
        }

        if(noButton.gameObject.activeSelf == true)
        {
            if(noDistanceToPlayer <= 2f)
            {
                gameScript.skipTutorial = true;
                startRapidFire = true;
                StartCoroutine(sceneTransition.FadeOutTransition());

                CreateParticleEffect(particleEffect, noButton.transform.position, Quaternion.Euler(Vector3.zero));

                soundPlay.PlayStartSound();

                DeactivateObject(yesButton.gameObject);
                DeactivateObject(noButton.gameObject);
                DeactivateObject(backButton.gameObject);
            }
        }

        if(backButton.gameObject.activeSelf == true)
        {
            if (backDistanceToPlayer <= 2f)
            {
                CreateParticleEffect(particleEffect, backButton.transform.position, Quaternion.Euler(Vector3.zero));

                soundPlay.PlayOptionSelectSound();

                DeactivateObject(yesButton.gameObject);
                DeactivateObject(noButton.gameObject);
                DeactivateObject(backButton.gameObject);
                DeactivateObject(tutorialText.gameObject);

                ActivateObject(startButton.gameObject);
                ActivateObject(gameModesButton.gameObject);
                ActivateObject(quitButton.gameObject);
            }
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
