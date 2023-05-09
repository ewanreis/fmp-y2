using System.Collections;
using UnityEngine;

public class ExplodeOnCollision : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private Transform explosionParent;

    private Collision2D col;
    private float explosionTimer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.IsChildOf(transform))
            return;
        col = collision;
        //Explode(0);
    }

    public void ChildCollision()
    {
        //Explode(0);
    }

    public void StartExplosion(float delay)
    {
        explosionTimer = 0;
        Invoke("Explosion", delay);
        Debug.Log("Explode 1");
    }

    private void Explosion()
    {
        GameObject obj = Instantiate(explosionPrefab, explosionParent.position, Quaternion.identity);
        obj.transform.parent = explosionParent;
        obj.transform.localPosition = Vector3.zero;
        Debug.Log("Explode 3");

        //Destroy(obj, 2f);
        Destroy(this.transform.parent.gameObject, 2f);
    }
}
