using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceBar : MonoBehaviour
{
    /* Written by Daniela
     * 
     */
    public Slider slider;
    public bool isHealthBar;
    public bool isCreep;
    public bool isEnemy;
    public bool isInUI;

    public TextMeshProUGUI healthText;

    private void Start()
    {
        UpdateHealthText();
    }

    public void SetValue(int value)
    {
        slider.value = value;
        UpdateHealthText();
    }

    public void SetMaxValue(int value)
    {
        slider.maxValue = value;
        slider.value = value;
    }

    public float GetMaxValue()
    {
        return slider.maxValue;
    }

    public float GetValue()
    {
        return slider.value;
    }

    public void UpdateHealthText()
    {
        if (isHealthBar && !isCreep && isInUI)
        {
            float percentage = GetValue() / GetMaxValue();
            healthText.text = (percentage * 100).ToString("f1") + "%";
        }
    }

}
