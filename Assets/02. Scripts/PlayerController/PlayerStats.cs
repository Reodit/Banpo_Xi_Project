using Invector.vCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public HealthBar healthBar;
    vThirdPersonController cc;
    private void Start()
    {
        cc = GetComponent<vThirdPersonController>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        cc.animator.CrossFadeInFixedTime("Damage", 0.1f);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            cc.animator.CrossFadeInFixedTime("Die", 0.1f);
            cc.isDie = true;
        }

    }

}
