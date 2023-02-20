using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> OnUpdateHealth;
    private static int currentHealth;

    public static int GetPlayerHealth() => currentHealth;

    public static void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        OnUpdateHealth.Invoke(currentHealth);
    }
}
