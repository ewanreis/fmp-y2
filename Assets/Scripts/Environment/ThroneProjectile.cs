using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneProjectile : MonoBehaviour
{
    public static event System.Action<Vector3> OnHit;
    [SerializeField] private float damage;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionParent;
    private Collider2D col;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log($"{other.gameObject}, {other.gameObject.tag}");
        Transform body;
        body = other.gameObject.transform.Find("Body");
        if(body == null)
        {
            Explode();
            return;
        }

        if(body.gameObject.tag == "Enemy")
        {
            Debug.Log("enemyHit");
            Explode();
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

        Destroy(this.gameObject);
    }

    private void InstantiateParticleSystem()
    {
        GameObject obj = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        obj.name = "ThroneProjectileExplosion";
        obj.transform.position = this.transform.position;
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
