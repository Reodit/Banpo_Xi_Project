using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int healthLevel = 100;
    public int maxHealth;
    public int currentHealth;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Damage : " + damage);

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Die");
        }
    }
}
