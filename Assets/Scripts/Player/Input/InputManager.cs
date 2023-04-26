using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static PlayerInput playerInput;

    private bool isMovePressed = false;
    private Vector2 lastMoveInput = Vector2.zero;

    private bool isPrimaryPressed = false;
    private bool isSecondaryPressed = false;
    public static bool usingController = false;

    // events for different input types
    public static event Action<Vector2> OnMoveInput;
    public static event Action OnPrimaryPressed;
    public static event Action OnSecondaryPressed;
    public static event Action OnPausePressed;
    public static event Action OnBestiaryPressed;
    public static event Action OnAchievementsPressed;
    public static event Action<Vector2> OnMoveHeld;
    public static event Action OnSkipSongPressed;
    public static event Action OnMountPressed;
    public static event Action OnPrimaryHeld;
    public static event Action OnSecondaryHeld;

    // controller only
    public static event Action<Vector2> OnRightStickMoved;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void FixedUpdate()
    {
        // check if move input is held down and invoke event
        if (isMovePressed)
            OnMoveHeld?.Invoke(lastMoveInput);
        if(isPrimaryPressed)
            OnPrimaryHeld?.Invoke();
        if(isSecondaryPressed)
            OnSecondaryHeld?.Invoke();
    }

    private void OnEnable()
    {
        // enable events & player input object
        playerInput.Enable();
        playerInput.Overworld.Walk.performed += OnMovePerformed;
        playerInput.Overworld.Walk.canceled += OnMoveCanceled;
        playerInput.Overworld.UsePrimary.performed += OnPrimaryPerformed;
        playerInput.Overworld.UseSecondary.performed += OnSecondaryPerformed;
        playerInput.Overworld.Pause.performed += OnPausePerformed;
        playerInput.Overworld.Bestiary.performed += OnBestiaryPerformed;
        playerInput.Overworld.Achievements.performed += OnAchievementsPerformed;
        playerInput.Overworld.SkipSong.performed += OnSkipSongPerformed;
        playerInput.Overworld.Mount.performed += OnMountPerformed;
        playerInput.Overworld.UsePrimary.canceled += OnPrimaryCanceled;
        playerInput.Overworld.UseSecondary.canceled += OnSecondaryCancelled;
        playerInput.Overworld.RightJoystick.performed += OnRightJoystickPerformed;

        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDisable()
    {
        // unregister events and disable player input object
        playerInput.Disable();
        playerInput.Overworld.Walk.performed -= OnMovePerformed;
        playerInput.Overworld.Walk.canceled -= OnMoveCanceled;
        playerInput.Overworld.UsePrimary.performed -= OnPrimaryPerformed;
        playerInput.Overworld.UseSecondary.performed -= OnSecondaryPerformed;
        playerInput.Overworld.Pause.performed -= OnPausePerformed;
        playerInput.Overworld.Bestiary.performed -= OnBestiaryPerformed;
        playerInput.Overworld.Achievements.performed -= OnAchievementsPerformed;
        playerInput.Overworld.SkipSong.performed -= OnSkipSongPerformed;
        playerInput.Overworld.Mount.performed -= OnMountPerformed;
        playerInput.Overworld.UsePrimary.canceled -= OnPrimaryCanceled;
        playerInput.Overworld.UseSecondary.canceled -= OnSecondaryCancelled;
        playerInput.Overworld.RightJoystick.performed -= OnRightJoystickPerformed;

        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // read movement input and invoke event when it is changed
        Vector2 moveInput = context.ReadValue<Vector2>();
        OnMoveInput?.Invoke(moveInput);
        isMovePressed = true;
        lastMoveInput = moveInput;
    }

    private void OnRightJoystickPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        OnRightStickMoved?.Invoke(direction);
    }

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // set move pressed flag to false when movement input is released
        isMovePressed = false;
        lastMoveInput = Vector2.zero;
    }

    private void OnPrimaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if primary input button pressed then invoke event if true
        bool primaryInput = context.ReadValue<float>() == 1 ? true : false;
        if (primaryInput)
        {
            OnPrimaryPressed?.Invoke();
            isPrimaryPressed = true;
        }
    }

    private void OnPrimaryCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isPrimaryPressed = false;
    }

    private void OnSecondaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if secondary input button pressed then invoke event if true
        bool secondaryInput = context.ReadValue<float>() == 1 ? true : false;
        if(secondaryInput)
        {
            OnSecondaryPressed?.Invoke();
            isSecondaryPressed = true;
        }
    }

    private void OnSecondaryCancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        isSecondaryPressed = false;
    }

    private void OnAchievementsPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if achievements menu input button pressed then invoke event if true
        bool pressed = context.ReadValue<float>() == 1 ? true : false;
        if(pressed)
            OnAchievementsPressed?.Invoke();
    }

    private void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if pause input button pressed then invoke event if true
        bool pressed = context.ReadValue<float>() == 1 ? true : false;
        if (pressed)
            OnPausePressed?.Invoke();
    }

    private void OnBestiaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if bestiary input button pressed then invoke event if true
        bool pressed = context.ReadValue<float>() == 1 ? true : false;
        if (pressed)
            OnBestiaryPressed?.Invoke();
    }

    private void OnSkipSongPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if skip button pressed then invoke event if true
        bool pressed = context.ReadValue<float>() == 1 ? true : false;
        if (pressed)
            OnSkipSongPressed?.Invoke();
    }

    private void OnMountPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        // check if skip button pressed then invoke event if true
        bool pressed = context.ReadValue<float>() == 1 ? true : false;
        if (pressed)
            OnMountPressed?.Invoke();
    }

    public static Vector2 GetMousePosition()
    {
        // return cursor position
        return playerInput.Overworld.CursorPosition.ReadValue<Vector2>();
    }

    public static Vector2 GetRightStickDirection()
    {
        return playerInput.Overworld.RightJoystick.ReadValue<Vector2>();
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        
        if (device is Gamepad && change != InputDeviceChange.Removed)
        {
            usingController = true;
            Debug.Log($"Using Controller");
        }
        else
        {
            usingController = false;
            //Debug.Log($"Using Keyboard");
        }
    }
}