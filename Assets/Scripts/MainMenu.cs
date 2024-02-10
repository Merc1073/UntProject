using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.ParticleSystem;

public class MainMenu : MonoBehaviour
{

    Transform startButton;
    Transform gameModesButton;
    Transform quitButton;

    public GameObject particleEffect;

    GenericPlaySound soundPlay;

    public bool startRapidFire = false;
    public bool startGrowth = false;

    public bool gameEnd = false;

    public bool startButtonActivated = false;
    public bool quitButtonActivated = false;

    private float timer;

    //public ParticleSystem startButtonParticle;
    //public ParticleSystem gameModesButtonParticle;
    //public ParticleSystem quitButtonParticle;

    public GameObject player;
    GameScript gameScript;

    public bool particOnce = true;

    FadeTransition sceneTransition;

    public float startDistanceToPlayer;
    public float gameModeDistanceToPlayer;
    public float quitDistanceToPlayer;

    void Start()
    {

        gameScript = FindObjectOfType<GameScript>();
        sceneTransition = FindObjectOfType<FadeTransition>();
        soundPlay = GetComponentInParent<GenericPlaySound>();
        
        startButton = transform.GetChild(0);
        gameModesButton = transform.GetChild(1);
        quitButton = transform.GetChild(2);
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


        if(startDistanceToPlayer <= 2f && particOnce)
        {
            startRapidFire = true;
            StartCoroutine(sceneTransition.FadeOutTransition());


            if(!startButtonActivated)
            {
                CreateParticleEffect(particleEffect, startButton.transform.position, Quaternion.Euler(Vector3.zero));
                startButtonActivated = true;
            }
            
            soundPlay.canPlaySound = true;

            DeactivateObject(startButton.gameObject);
            DeactivateObject(gameModesButton.gameObject);
            DeactivateObject(quitButton.gameObject);

        }

        if(quitDistanceToPlayer <= 2f)
        {
            StartCoroutine(sceneTransition.FadeOutTransition());

            if(!quitButtonActivated)
            {
                CreateParticleEffect(particleEffect, quitButton.transform.position, Quaternion.Euler(Vector3.zero));
                quitButtonActivated = true;
            }

            soundPlay.canPlaySound = true;

            DeactivateObject(startButton.gameObject);
            DeactivateObject(gameModesButton.gameObject);
            DeactivateObject(quitButton.gameObject);

            gameEnd = true;

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

}
