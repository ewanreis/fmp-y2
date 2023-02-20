using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThroneController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InputManager.OnMoveHeld += HandleMoveInput;
    }

    private void OnDestroy()
    {
        InputManager.OnMoveHeld -= HandleMoveInput;
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        if (Mathf.Abs(rb.velocity.x) < speed)
        {
            if (moveInput.x > 0)
            {
                rb.AddForce(Vector2.right * 50f);
            }
            if (moveInput.x < 0)
            {
                rb.AddForce(-Vector2.right * 50f);
            }
        }
    }
}
