using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountableObject : MonoBehaviour
{
    public GameObject player;

    // mount point on the object where the player will be positioned.
    public Transform mountPoint;

    private bool isMounted = false;

    private void Start()
    {
        InputManager.OnPrimaryPressed += ToggleMount;
    }

    private void ToggleMount()
    {
        if(!isMounted)
            Mount();
        else
            Unmount();
    }

    private void Mount()
    {
        player.transform.position = mountPoint.position;
        player.transform.rotation = mountPoint.rotation;
        player.transform.parent = transform;

        // disable players movement and rotation
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<PlayerMovement>().enabled = false;

        isMounted = true;
    }

    private void Unmount()
    {
        player.transform.parent = null;
        player.transform.position = transform.position + Vector3.up;

        // enable players movement and rotation.
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<PlayerMovement>().enabled = true;

        isMounted = false;
    }
}
