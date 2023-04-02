using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureZone : MonoBehaviour
{
    public string enemyTag = "Enemy";

    [SerializeField] private int damagePerTick = 5; // damage to deal per tick
    [SerializeField] private float tickSpeed = 1f; // speed of each tick

    private float damageTimer = 0f;

    private void Start()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(enemyTag))
        {
            damageTimer += Time.deltaTime; // increment damage timer

            if (damageTimer >= tickSpeed) // if tick time has passed threshold
            {
                PlayerHealth.DamagePlayer(damagePerTick);
                damageTimer = 0f;
            }
        }
    }
}
