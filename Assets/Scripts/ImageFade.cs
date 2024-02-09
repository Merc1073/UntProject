using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{

    //public Image[] imageToFade;

    public Image imageToFade1;
    public Image imageToFade2;


    public float fadeInDuration;

    private void Start()
    {
        //for(int i = 0; i < imageToFade.Length; i++)
        //{
        //    imageToFade[i].GetComponent<Image>();
        //}

        if(imageToFade1 == null)
        {
            imageToFade1 = GetComponent<Image>();
        }

        if(imageToFade2 == null)
        {
            imageToFade2 = GetComponent<Image>();
        }

        StartCoroutine(FadeInImage1());
        StartCoroutine(FadeInImage2());

    }


    //private IEnumerator FadeInImage()
    //{
    //    for(int i = 0; i < imageToFade.Length; i++)
    //    {
    //        Color color = imageToFade[i].color;
    //        color.a = 0f;
    //        imageToFade[i].color = color;

    //        for(float t = 0; t < fadeInDuration; t += Time.deltaTime)
    //        {
    //            color.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
    //            imageToFade[i].color = color;
    //            yield return null;
    //        }

    //        color.a = 1f;
    //        imageToFade[i].color = color;
    //    }

    //}

    private IEnumerator FadeInImage1()
    {

        Color color1 = imageToFade1.color;
        color1.a = 0f;
        imageToFade1.color = color1;

        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            color1.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            imageToFade1.color = color1;
            yield return null;
        }

        color1.a = 1f;
        imageToFade1.color = color1;

    }

    private IEnumerator FadeInImage2()
    {
        Color color2 = imageToFade2.color;
        color2.a = 0f;
        imageToFade2.color = color2;

        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            color2.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            imageToFade2.color = color2;
            yield return null;
        }

        color2.a = 1f;
        imageToFade2.color = color2;
    }


}
