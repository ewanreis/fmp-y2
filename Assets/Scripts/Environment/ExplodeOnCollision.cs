using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnCollision : MonoBehaviour
{
    //* This script makes the attached object explode and destroy on collision impact
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionParent;
    [SerializeField] private float explosionRange;
    [SerializeField] private string entityTag;
    [SerializeField] private LayerMask entityLayer;
    [SerializeField] private Transform head;
    private Collision2D col;
    List<Collider2D> colliders;
    private float explosionTimer;
    private List<GameObject> entitiesInExplosion;
    private bool exploding;

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
        if(exploding)
            return;
        
        exploding = true;
        InstantiateParticleSystem();
        SetSpritesHidden();
        GetEntitiesInRadius();

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
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = entityLayer;
        contactFilter.useLayerMask = true;

        colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(head.transform.position, explosionRange, contactFilter, colliders);
        foreach (Collider2D collider in colliders)
            if (Vector3.Distance(head.transform.position, collider.transform.position) <= explosionRange)
            {
                entitiesInExplosion.Add(collider.gameObject);
                Health entityHealth = collider.gameObject.GetComponent<Health>();
                entityHealth?.Damage(3);
                //Debug.Log(collider.gameObject);
            }
    }

    private void SetSpritesHidden()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
        SpriteRenderer[] spritesInChildren = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in spritesInChildren) 
            sprite.enabled = false;
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(head.transform.position, explosionRange);
    }
}
