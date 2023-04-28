using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject entity;
    [SerializeField] private float maxHealth;
    [SerializeField] private float regenRate;
    [SerializeField] private float regenDelay;

    private float health;

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;

        Debug.Log(health);

        if(health < 0)
            health = 0;

        if(health == 0)
            Die();
    }

    public void Die()
    {
        Destroy(entity);
    }
}
