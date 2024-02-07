using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    private float target = 1f;

    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;

    private void Start()
    {
        CheckHealthbarGradientAmount();
    }


    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        target = image.fillAmount;
        image.fillAmount = currentHealth / maxHealth;

        CheckHealthbarGradientAmount();
    }

    private void CheckHealthbarGradientAmount()
    {
        image.color = gradient.Evaluate(target);
    }

}
