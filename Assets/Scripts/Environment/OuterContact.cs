using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterContact : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform != transform.parent)
            transform.parent.GetComponent<ExplodeOnCollision>().ChildCollision();
    }
}
