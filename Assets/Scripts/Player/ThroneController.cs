using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ThroneController : MonoBehaviour
{
    public float speed = 1f;
    private Rigidbody2D rb;
    private Collider2D throneCollider;

    private bool isFrozen = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        throneCollider = GetComponent<Collider2D>();

        InputManager.OnMoveHeld += HandleMoveInput;
        MountableObject.OnMount += UnfreezeMovement;
        MountableObject.OnUnmount += FreezeMovement;

        FreezeMovement();
    }

    private void OnDestroy()
    {
        InputManager.OnMoveHeld -= HandleMoveInput;
    }

    private void FreezeMovement()
    {
        isFrozen = true;

    }

    private void UnfreezeMovement()
    {
        isFrozen = false;
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        if(isFrozen)
            return;

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
