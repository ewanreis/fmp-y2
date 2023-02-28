using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Collider2D playerCollider;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelerationForce;
    [SerializeField] private float deccelerationForce;

    private bool isFrozen = false;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        // handle player walk
        InputManager.OnMoveHeld += HandleMoveInput;

        // handle player mounting a rideable object
        MountableObject.OnMount += DisablePlayerPhysics;
        MountableObject.OnMount += FreezePlayerMovement;

        // handle player dismounting
        MountableObject.OnUnmount += EnablePlayerPhysics;
        MountableObject.OnUnmount += UnfreezePlayerMovement;
    }

    private void DisablePlayerPhysics()
    {
        playerRigidbody.isKinematic = true;
        playerRigidbody.velocity = Vector2.zero;
        playerCollider.enabled = false;
    }

    private void EnablePlayerPhysics()
    {
        playerRigidbody.isKinematic = false;
        playerCollider.enabled = true;
    }

    private void FreezePlayerMovement()
    {
        isFrozen = true;
    }

    private void UnfreezePlayerMovement()
    {
        isFrozen = false;
    }

    private void HandlePlayerMovement()
    {
        if(isFrozen)
            return;
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        if(isFrozen)
            return;

        playerRigidbody.AddForce(new Vector2(moveInput.x * moveSpeed, 0));



    }
}