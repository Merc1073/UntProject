using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{

    public Image imageToFade;
    public float fadeInDuration;
    public float fadeOutDuration;

    MainPlayer player;
    MainMenu menu;

    private void Start()
    {
        player = FindObjectOfType<MainPlayer>();
        menu = FindObjectOfType<MainMenu>();

        player.canMove = true;

        StartCoroutine(FadeInTransition());
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

        if(menu.startRapidFire == true)
        {
            SceneManager.LoadScene("Rapid Fire");
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
