using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private float target = 1f;

    [SerializeField] private float timeToDrain = 0.25f;

    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;

    private Coroutine drainHealthBar;

    private void Start()
    {
        CheckHealthbarGradientAmount();
    }


    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = currentHealth / maxHealth;

        drainHealthBar = StartCoroutine(DrainHealthBar());

        CheckHealthbarGradientAmount();
    }

    private IEnumerator DrainHealthBar()
    {

        float fillAmount = image.fillAmount;

        float elapsedTime = 0f;
        while(elapsedTime < timeToDrain)
        {
            elapsedTime += Time.deltaTime;

            image.fillAmount = Mathf.Lerp(fillAmount, target, (elapsedTime / timeToDrain));

            yield return null;
        }
    }

    private void CheckHealthbarGradientAmount()
    {
        image.color = gradient.Evaluate(target);
    }


}
