using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    Vector3 originalScale;
    Vector3 previous;
    Vector3 velocity;
    private void Start()
    {
        originalScale = gameObject.transform.localScale;
    }

    private void FixedUpdate()
    {
        velocity = (transform.position - previous) * 10;
        previous = transform.position;

        if(velocity.x == 0)
            return;

        if(velocity.x < -0.1f)
            gameObject.transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);

        if(velocity.x > 0.1f)
            gameObject.transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
    }
}
