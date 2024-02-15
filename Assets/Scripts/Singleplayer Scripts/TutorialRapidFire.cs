using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRapidFire : MonoBehaviour
{

    GameScript gameScript;
    public GameObject tutorial;

    [Header("Section 1")]
    public TMP_Text stage1;

    [Header("Section 2")]
    public TMP_Text collectPowerUps;
    public TMP_Text powerUpsText;

    public GameObject magnetPowerUp;
    public GameObject tripleBulletPowerUp;

    [Header("Section 3")]
    public TMP_Text stage3;

    [Header("Section 4")]
    public TMP_Text Three;
    public TMP_Text Two;
    public TMP_Text One;
    public TMP_Text Go;
        

    [Header("Section Other")]
    public float timer;
    public float timer2;

    public float tutorialStartDelay;

    private void Start()
    {

        gameScript = FindObjectOfType<GameScript>();

        //if(gameScript.skipTutorial)
        //{
            stage1.gameObject.SetActive(false);
        //}

        //stage2
        collectPowerUps.gameObject.SetActive(false);
        powerUpsText.gameObject.SetActive(false);

        magnetPowerUp.gameObject.SetActive(false);
        tripleBulletPowerUp.gameObject.SetActive(false);

        //stage3
        stage3.gameObject.SetActive(false);

        //stage4
        Three.gameObject.SetActive(false);
        Two.gameObject.SetActive(false);
        One.gameObject.SetActive(false);
        Go.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!gameScript) return;
        if(!gameScript.skipTutorial)
        {
            timer += Time.deltaTime;

            if(timer >= 0f + tutorialStartDelay)
            {
                stage1.gameObject.SetActive(true);
            }

            if (timer >= 3.5f + tutorialStartDelay)
            {
                stage1.gameObject.SetActive(false);
            }

            //Stage2
            if (timer >= 4.0f + tutorialStartDelay)
            {
                collectPowerUps.gameObject.SetActive(true);
                powerUpsText.gameObject.SetActive(true);

                magnetPowerUp.gameObject.SetActive(true);
                tripleBulletPowerUp.gameObject.SetActive(true);
            }


            if (timer >= 10.5f + tutorialStartDelay)
            {
                collectPowerUps.gameObject.SetActive(false);
                powerUpsText.gameObject.SetActive(false);

                magnetPowerUp.gameObject.SetActive(false);
                tripleBulletPowerUp.gameObject.SetActive(false);
            }

            //stage3
            if (timer >= 11.0f + tutorialStartDelay)
            {

                stage3.gameObject.SetActive(true);

            }

            if (timer >= 17.5f + tutorialStartDelay)
            {
                stage3.gameObject.SetActive(false);
            }

            if (timer >= 18.0f + tutorialStartDelay)
            {
                Three.gameObject.SetActive(true);

                if (timer >= 19.0f + tutorialStartDelay)
                {
                    Three.gameObject.SetActive(false);
                    Two.gameObject.SetActive(true);

                    if (timer >= 20.0f + tutorialStartDelay)
                    {
                        Two.gameObject.SetActive(false);
                        One.gameObject.SetActive(true);

                        if (timer >= 21.0f + tutorialStartDelay)
                        {
                            One.gameObject.SetActive(false);
                            Go.gameObject.SetActive(true);
                            gameScript.hasRapidFireModeStarted = true;

                            if (timer >= 22.0f + tutorialStartDelay)
                            {
                                Go.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }

            if (timer >= 25.0f + tutorialStartDelay)
            {
                Destroy(tutorial.gameObject);
            }
        }

        else
        {
            timer2 += Time.deltaTime;

            if (timer2 >= 2.0f)
            {
                Three.gameObject.SetActive(true);

                if (timer2 >= 3.0f)
                {
                    Three.gameObject.SetActive(false);
                    Two.gameObject.SetActive(true);

                    if (timer2 >= 4.0f)
                    {
                        Two.gameObject.SetActive(false);
                        One.gameObject.SetActive(true);

                        if (timer2 >= 5.0f)
                        {
                            One.gameObject.SetActive(false);
                            Go.gameObject.SetActive(true);
                            gameScript.hasRapidFireModeStarted = true;

                            if (timer2 >= 6.0f)
                            {
                                Go.gameObject.SetActive(false);
                            }
                        }
                    }
                }
            }

            if (timer2 >= 9.0f)
            {
                Destroy(tutorial.gameObject);
            }
        }
    }
}
