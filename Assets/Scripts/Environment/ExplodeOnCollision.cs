using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionParent;
    [SerializeField] private float explosionRange;
    [SerializeField] private string entityTag;
    [SerializeField] private LayerMask entityLayer;
    private Collision2D col;
    private Collider2D[] colliders;
    private float explosionTimer;
    private List<GameObject> entitiesInExplosion;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.IsChildOf(transform))
            return;
        col = collision;
    }

    public void ChildCollision()
    {
        explosionTimer = 0;
        Invoke("Explosion", 0.1f);
    }

    public void StartExplosion(float delay)
    {
        explosionTimer = 0;
        Invoke("Explosion", delay);
    }

    private void Explosion()
    {
        InstantiateParticleSystem();
        SetSpritesHidden();
        GetEntitiesInRadius();
        DamageEntitiesInRange();

        Destroy(this.transform.parent.gameObject, 4f);
    }

    private void InstantiateParticleSystem()
    {
        GameObject obj = Instantiate(explosionPrefab, explosionParent.position, Quaternion.identity);
        obj.transform.parent = explosionParent;
        obj.transform.localPosition = Vector3.zero;
    }

    private void GetEntitiesInRadius()
    {
        entitiesInExplosion = new List<GameObject>();
        colliders = Physics2D.OverlapCircleAll(this.transform.position, explosionRange, entityLayer);

        foreach (Collider2D collider in colliders)
            if (Vector3.Distance(this.transform.position, collider.transform.position) <= explosionRange)
                entitiesInExplosion.Add(collider.gameObject);
    }

    private void SetSpritesHidden()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        SpriteRenderer[] spritesInChildren = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in spritesInChildren) 
            sprite.enabled = false;
    }

    private void DamageEntitiesInRange()
    {
        if(entitiesInExplosion.Count > 0)
        {
            List<Health> entityHealthList = new List<Health>();
            foreach (GameObject entity in entitiesInExplosion)
            {
                Health entityHealth = GetComponent<Health>();
                entityHealth?.Damage(5);
            }
        }
    }
}
