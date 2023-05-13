using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneProjectile : MonoBehaviour
{
    //* Manages the shot projectile from the throne to explode on impact and damage enemies
    public static event System.Action<Vector3> OnHit;
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosionPrefab;
    private Collider2D col;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Transform body;
        body = other.gameObject.transform.Find("Body");
        if(body == null)
        {
            Explode();
            return;
        }

        if(body.gameObject.tag == "Enemy")
        {
            //Debug.Log("enemyHit");
            Explode();
            Health enemyHealth = body.gameObject.GetComponent<Health>();
            enemyHealth.Damage(damage);
        }
        else
        {
            Explode();
        }
    }

    private void Explode()
    {
        InstantiateParticleSystem();
        //SetSpritesHidden();
        OnHit.Invoke(this.transform.position);
        Destroy(this.gameObject);
    }


    private void InstantiateParticleSystem()
    {
        GameObject obj = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        obj.name = "ThroneProjectileExplosion";
        obj.transform.position = this.transform.position;
        Destroy(obj, 4f);
    }


    private void SetSpritesHidden()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        SpriteRenderer[] spritesInChildren = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in spritesInChildren) 
            sprite.enabled = false;
    }
}
