using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BehaviorTree;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject entity;
    [SerializeField] private float maxHealth;
    [SerializeField] private float regenRate;
    [SerializeField] private float regenDelay;


    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    private float health;
    private DamageFlash damageFlash;

    private IEnumerator PassiveRegen()
    {
        yield return new WaitForSeconds(regenDelay);

        while (health < maxHealth)
        {
            Heal(regenRate * Time.deltaTime);
            UpdateHealthBar();
            yield return null;
        }
    }

    void OnEnable()
    {
        damageFlash = this.GetComponent<DamageFlash>();
        health = maxHealth;
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
        Destroy(entity);
    }
}
