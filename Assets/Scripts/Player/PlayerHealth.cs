using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> OnUpdateHealth;
    private static int currentHealth;
    [SerializeField] private int startingHealth;

    public static int GetPlayerHealth() => currentHealth;

    private void Start()
    {
        currentHealth = startingHealth;
    }

    public static void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
            currentHealth = 0;

        OnUpdateHealth.Invoke(currentHealth);
    }
}
