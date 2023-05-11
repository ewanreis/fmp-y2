using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MountableObject : MonoBehaviour
{
    //* This script manages the mounting and dismounting of the throne and any other mountable objects

    [SerializeField] private GameObject player;
    [SerializeField] private Transform mountPoint;
    [SerializeField] private float yOffset;

    public static event Action OnMount;
    public static event Action OnUnmount;

    private bool isMounted = false;
    private bool canMount = false;

    private void OnEnable()
    {
        InputManager.OnMountPressed += ToggleMount;
        TooltipThrone.OnTooltipShow += EnableMount;
        TooltipThrone.OnTooltipHide += DisableMount;
    }

    private void OnDisable()
    {
        InputManager.OnMountPressed -= ToggleMount;
        TooltipThrone.OnTooltipShow -= EnableMount;
        TooltipThrone.OnTooltipHide -= DisableMount;
    }

    private void ToggleMount()
    {
        // stop unwanted keypresses when paused
        if(PauseMenu.paused)
            return;

        if(!isMounted && canMount)
            Mount();
        else if(isMounted)
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

    private void EnableMount() =>  canMount = true;
    private void DisableMount() => canMount = false;

    private void FixedUpdate()
    {
        if(!isMounted)
            return;
        
        player.transform.position = mountPoint.position + new Vector3(0, yOffset, 0);
        player.transform.rotation = mountPoint.rotation;
    }

    public bool GetMountState() => isMounted;
}
