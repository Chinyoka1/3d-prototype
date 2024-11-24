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
    private void UpdateSliderValue(int health)
    {
        slider.value = health;
        healthText.text = health + " / " + damageable.maxHealth;
    }
}
