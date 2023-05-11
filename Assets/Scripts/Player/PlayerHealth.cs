using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    //* Manages the player and castle's shared health
    public static event Action<int> OnUpdateHealth;
    public static event Action OnLowHealth;
    public static event Action OnCriticalHealth;
    public static event Action OnDeath;

    private static int currentHealth;
    [SerializeField] private int startingHealth;
    private static int lowHealth;
    private static int criticalHealth;
    private static bool isAlive = true;

    public static int GetPlayerHealth() => currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
        lowHealth = startingHealth / 2;
        criticalHealth = startingHealth / 3;
        isAlive = true;
    }

    public static void DamagePlayer(int damage)
    {
        currentHealth -= damage;

        if(currentHealth < 0)
            currentHealth = 0;

        if(currentHealth == 0 && isAlive)
        {
            OnDeath?.Invoke();
            isAlive = false;
        }

        if(currentHealth <= lowHealth && currentHealth > criticalHealth)
            OnLowHealth?.Invoke();

        if(currentHealth <= criticalHealth)
            OnCriticalHealth?.Invoke();

        OnUpdateHealth?.Invoke(currentHealth);
    }
}
