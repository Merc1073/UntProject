using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTextFade : MonoBehaviour
{

    public Text textToFade1;

    public float fadeInDuration;

    private void Start()
    {

        StartCoroutine(FadeInText1());

    }


    private IEnumerator FadeInText1()
    {
        Color color1 = textToFade1.color;
        color1.a = 0f;
        textToFade1.color = color1;

        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            color1.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            textToFade1.color = color1;
            yield return null;
        }

        color1.a = 1f;
        textToFade1.color = color1;
    }

    private IEnumerator FadeInText2()
    {
        Color color1 = textToFade1.color;
        color1.a = 0f;
        textToFade1.color = color1;

        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            color1.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            textToFade1.color = color1;
            yield return null;
        }

        color1.a = 1f;
        textToFade1.color = color1;
    }
}
