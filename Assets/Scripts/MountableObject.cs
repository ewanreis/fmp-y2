using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MountableObject : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Transform mountPoint;
    [SerializeField] private float yOffset;

    public static event Action OnMount;
    public static event Action OnUnmount;

    private bool isMounted = false;

    private void Start()
    {
        InputManager.OnPrimaryPressed += ToggleMount;
    }

    private void ToggleMount()
    {
        // stop unwanted keypresses when paused
        if(PauseMenu.paused)
            return;

        if(!isMounted)
            Mount();
        else
            Unmount();
    }

    private void Mount()
    {
        isMounted = true;

        // disable players movement and rotation
        OnMount.Invoke();

    }

    private void Unmount()
    {
        isMounted = false;

        player.transform.parent = null;
        player.transform.position = transform.position + Vector3.up;
        player.transform.rotation = Quaternion.identity;

        // enable players movement and rotation.
        OnUnmount.Invoke();

    }

    private void FixedUpdate()
    {
        if(!isMounted)
            return;
        
        player.transform.position = mountPoint.position + new Vector3(0, yOffset, 0);
        player.transform.rotation = mountPoint.rotation;
    }
}
