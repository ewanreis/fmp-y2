using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BehaviorTree;

public class Health : MonoBehaviour
{
    //* This code will manage the health and healthbar of the attached entity
    public static event System.Action<Creature, bool> OnDeath;

    [SerializeField] private Creature creatureType;

    [SerializeField] private GameObject entity;
    [SerializeField] private float maxHealth;
    [SerializeField] private float regenRate;
    [SerializeField] private float regenDelay;


    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isEnemy;

    private float health;
    private float regenCounter;
    private DamageFlash damageFlash;

    private IEnumerator PassiveRegen()
    {
        while (health < maxHealth)
        {
            regenCounter += Time.deltaTime;
            if(regenCounter >= regenDelay)
            {
                Heal(regenRate * Time.deltaTime);
                UpdateHealthBar();
            }
            
            yield return null;
        }
    }

    public void IncreaseMaxHealth(float multiplier)
    {
        maxHealth *= multiplier;
        health = maxHealth;
    }

    void OnEnable()
    {
        damageFlash = this.GetComponent<DamageFlash>();
        health = maxHealth;
        isEnemy = (this.gameObject.tag == "Enemy") ? true : false;
        UpdateHealthBar();
        StartCoroutine(PassiveRegen());
    }

    void OnDisable()
    {
        StopCoroutine(PassiveRegen());
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        regenCounter = 0;

        StopCoroutine(PassiveRegen());

        if(health < 0)
            health = 0;

        if(health == 0)
            Die();

        damageFlash.Flash();
        UpdateHealthBar();
        StartCoroutine(PassiveRegen());
    }

    public void Heal(float healAmount)
    {
        health += healAmount;

        if (health > maxHealth)
            health = maxHealth;

        UpdateHealthBar();
    }

    void LateUpdate()
    {
        if (target != null)
            healthBar.transform.position = Camera.main.WorldToScreenPoint(target.position + offset);
    }


    void UpdateHealthBar()
    {
        if(health == maxHealth)
            healthBar.gameObject.SetActive(false);
        else
            healthBar.gameObject.SetActive(true);

        float ratio = health / maxHealth;
        healthBar.value = ratio;
    }

    public void Die()
    {
        OnDeath?.Invoke(creatureType, isEnemy);
        Destroy(entity);
    }
}
