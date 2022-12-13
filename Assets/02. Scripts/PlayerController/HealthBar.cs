using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Slider staminaBar;

    private void Start()
    {
        healthBar = GetComponentInChildren<Slider>();
        staminaBar = GetComponentInChildren<Slider>();
    }

    #region Health
    public void SetMaxHealth(int maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }
    public void SetCurrentHealth(int currentHealth)
    {
        healthBar.value = currentHealth;
    }
    #endregion
    #region Stamina
    public void SetMaxStamina(int maxStamina)
    {
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }
    public void SetCurrentStamina(int currentStamina)
    {
        staminaBar.value = currentStamina;
    }
    #endregion

}
