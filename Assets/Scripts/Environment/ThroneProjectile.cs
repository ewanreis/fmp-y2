using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThroneProjectile : MonoBehaviour
{
    public static event System.Action<Vector3> OnHit;
    [SerializeField] private float damage;
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
        OnHit.Invoke(this.transform.position);
        Destroy(this.gameObject);
    }
}
