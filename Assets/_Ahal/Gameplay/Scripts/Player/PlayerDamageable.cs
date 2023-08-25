using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageble : DamageableComponent
{
    [SerializeField] private PlayerHealthUI playerHealthUI;
    [SerializeField] int maxHealth = 3;
    
    private int currentHealth = 0;

    protected void Awake()
    {
        SetHealth(maxHealth);
    }
    
    public override void OnDamage()
    {
        SetHealth(currentHealth-1);
        
        if (currentHealth <= 0)
        {
            // TODO: Temporary demo, should have values according to Animations or something~
            this.MonoWaitForSeconds(DeathHandler, 2f);
        }
    }

    private void SetHealth(int value)
    {
        currentHealth = value;
        playerHealthUI.SetHealth(currentHealth);
    }

    private void DeathHandler()
    {
        // TODO: call a different system
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // TODO: Death Animation
    }
}
