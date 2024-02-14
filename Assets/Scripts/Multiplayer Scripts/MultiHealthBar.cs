using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MultiHealthBar : NetworkBehaviour
{
    //private float target = 1f;

    //[SerializeField] private float timeToDrain = 0.25f;
    //[SerializeField] private float lerpSpeed;

    //[SerializeField] private Image image;
    //[SerializeField] private Gradient gradient;

    private void OnEnable()
    {
        //GetComponent<MultiMainPlayer>().currentHealth.OnValueChanged += HealthChanged();
    }


    private void OnDisable()
    {
        
    }

    private void HealthChanged(int previousValue, int newValue)
    {
        //image.fillAmount = Mathf.Lerp(image.fillAmount, target, (elapsedTime / timeToDrain) * lerpSpeed);
    }

    //private Coroutine drainHealthBar;

    //private void Start()
    //{
    //    CheckHealthbarGradientAmount();
    //}

    //public void Update()
    //{
    //    //lerpSpeed = lerpSpeed * Time.deltaTime;
    //}

    //public void UpdateHealthBar(float maxHealth, float currentHealth)
    //{
    //    target = currentHealth / maxHealth;

    //    drainHealthBar = StartCoroutine(DrainHealthBar());

    //    CheckHealthbarGradientAmount();
    //}

    //private IEnumerator DrainHealthBar()
    //{

    //    float fillAmount = image.fillAmount;

    //    float elapsedTime = 0f;
    //    while (elapsedTime < timeToDrain)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        lerpSpeed += 3f * Time.deltaTime;
    //        image.fillAmount = Mathf.Lerp(fillAmount, target, (elapsedTime / timeToDrain) * lerpSpeed);

    //        yield return null;
    //    }
    //}

    //private void CheckHealthbarGradientAmount()
    //{
    //    image.color = gradient.Evaluate(target);
    //}
}
