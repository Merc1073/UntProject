using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagnetPowerBar : MonoBehaviour
{

    //[SerializeField] private float timeToDrain = 0.25f;
    //[SerializeField] private float lerpSpeed;

    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;


    private void Start()
    {
        //CheckHealthbarGradientAmount();
    }

    public void Update()
    {
        
    }

    public void UpdateMagnetBar(float maxPower, float currentPower)
    {
        image.fillAmount = currentPower / maxPower;

        //drainMagnetBar = StartCoroutine(DrainMagnetBar());

        //CheckHealthbarGradientAmount();
    }

    //private IEnumerator DrainMagnetBar()
    //{

    //    float fillAmount = image.fillAmount;

    //    float elapsedTime = 0f;
    //    while (elapsedTime < timeToDrain)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        //lerpSpeed += 3f * Time.deltaTime;
    //        image.fillAmount = Mathf.Lerp(fillAmount, target, (elapsedTime / timeToDrain));

    //        yield return null;
    //    }
    //}

    //private void CheckHealthbarGradientAmount()
    //{
    //    image.color = gradient.Evaluate(target);
    //}
}
