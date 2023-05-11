using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterContact : MonoBehaviour
{
    //* Sends collision messages upwards to the parent object
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform != transform.parent)
        {
            transform.parent.GetComponent<ExplodeOnCollision>().ChildCollision();
        }
    }
}
