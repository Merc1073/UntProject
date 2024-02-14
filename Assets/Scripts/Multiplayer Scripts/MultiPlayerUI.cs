using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiPlayerUI : MonoBehaviour
{

    [SerializeField] Image HealthUI;


    private void OnEnable()
    {
        GetComponent<MultiHealthState>().HealthPoint.OnValueChanged += HealthChange;
    }


    private void OnDisable()
    {
        GetComponent<MultiHealthState>().HealthPoint.OnValueChanged -= HealthChange;
    }

    private void HealthChange(float previousValue, float newValue)
    {
        HealthUI.fillAmount = newValue / 5f;
    }

}
