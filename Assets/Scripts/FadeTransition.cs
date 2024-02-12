using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{

    public Image imageToFade;

    public float fadeTimer;

    public float fadeDelay;

    public float fadeInDuration;
    public float fadeOutDuration;

    bool hasFadedOnce = false;

    MainPlayer player;
    MainMenu menu;

    private void Start()
    {
        player = FindObjectOfType<MainPlayer>();
        menu = FindObjectOfType<MainMenu>();

        if(player)
        {
            player.canMove = false;
        }

        imageToFade.color = new Color(0f, 0f, 0f);

        //StartCoroutine(FadeInTransition());
    }

    private void Update()
    {
        fadeTimer += Time.deltaTime;

        if(fadeTimer >= fadeDelay && !hasFadedOnce)
        {
            if(player)
            {
                player.canMove = true;
            }
            StartCoroutine(FadeInTransition());
            hasFadedOnce = true;
        }

    }


    public IEnumerator FadeInTransition()
    {
        Color originalColor = imageToFade.color;
        float currentTime = 0f;

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeInDuration);
            imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

    }


    public IEnumerator FadeOutTransition()
    {
        player.canMove = false;
        Color originalColor = imageToFade.color;
        float currentTime = 0f;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1f, currentTime / fadeOutDuration);
            imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        if(menu.gameModeIsRapidFire == true)
        {
            SceneManager.LoadScene("Rapid Fire");
        }

        if(menu.gameModeIsGrowth == true)
        {
            SceneManager.LoadScene("Growth");
        }

        if(menu.gameEnd == true)
        {
            Debug.Log("Game End");
            Application.Quit();
        }

    }

    //public void FadeOutTransition()
    //{
    //    Color originalColor = imageToFade.color;
    //    float currentTime = 0f;

    //    while (currentTime < fadeDuration)
    //    {
    //        currentTime += Time.deltaTime;
    //        float alpha = Mathf.Lerp(0, 1f, currentTime / fadeDuration);
    //        imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    //    }

    //    imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    //}

}
