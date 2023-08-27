using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDamageble : DamageableComponent
{
    [SerializeField] private PlayerHealthUI playerHealthUI;
    [SerializeField] private PlayerMovement2D playerMovement2D;
    [SerializeField] private PlayerAnimationController playerAnimationController;
    
    [SerializeField] int maxHealth = 3;
    [SerializeField] float hurtMovementCooldown;
    
    private int currentHealth = 0;

    protected void Awake()
    {
        SetHealth(maxHealth);
    }
    
    public override void OnDamage()
    {
        SetHealth(currentHealth - 1);
        
        if (currentHealth <= 0)
        {
            playerAnimationController.SetDeath();
            playerMovement2D.disableControls = true;
            this.MonoWaitForSeconds(DeathHandler, 2f);
            return;
        }
        
        playerAnimationController.SetHurt();
        playerMovement2D.disableControls = true;
        this.MonoWaitForSeconds(
            () => playerMovement2D.disableControls = false,
            hurtMovementCooldown);
    }

    private void SetHealth(int value)
    {
        currentHealth = value;
        playerHealthUI.SetHealth(currentHealth);
    }

    private void DeathHandler()
    {
        // TODO: call a different system
        // Forgive me Mitzi
        SceneManager.LoadScene("MainMenu");
        // TODO: Death Animation
    }
}
