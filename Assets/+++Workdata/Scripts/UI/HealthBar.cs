using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]private Damageable damageable;
    private Slider slider;
    private TextMeshProUGUI healthText;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        healthText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void OnEnable()
    {
        damageable.onHealthChanged.AddListener(UpdateSliderValue);
        slider.maxValue = damageable.maxHealth;
        slider.value = damageable.GetHealth();
    }

    private void UpdateSliderValue(int health)
    {
        slider.value = health;
        
        if (healthText)
        {
            healthText.text = health + " / " + damageable.maxHealth;
        }
    }
}
