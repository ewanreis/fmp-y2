using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //* Manages the user's input for keyboard and mouse or controller
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
    => isMovePressed = GetCompositeStatus(context, OnMoveInput, out lastMoveInput);

    private void OnMoveCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => isMovePressed = GetCompositeStatus(context, OnMoveInput, out lastMoveInput);

    private void OnRightJoystickPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => GetCompositeStatus(context, OnRightStickMoved, out _);

    private void OnPrimaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => isPrimaryPressed = GetButtonStatus(context, OnPrimaryPressed);

    private void OnPrimaryCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => isPrimaryPressed = GetButtonStatus(context);

    private void OnSecondaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => isSecondaryPressed = GetButtonStatus(context, OnSecondaryPressed);

    private void OnSecondaryCancelled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => isSecondaryPressed = GetButtonStatus(context);

    private void OnAchievementsPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => GetButtonStatus(context, OnAchievementsPressed);

    private void OnPausePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => GetButtonStatus(context, OnPausePressed);

    private void OnBestiaryPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => GetButtonStatus(context, OnBestiaryPressed);

    private void OnSkipSongPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => GetButtonStatus(context, OnSkipSongPressed);

    private void OnMountPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    => GetButtonStatus(context, OnMountPressed);

    public static Vector2 GetMousePosition()
    {
        return playerInput.Overworld.CursorPosition.ReadValue<Vector2>();
    }

    public static Vector2 GetRightStickDirection()
    {
        return playerInput.Overworld.RightJoystick.ReadValue<Vector2>();
    }

    private bool GetButtonStatus(InputAction.CallbackContext context)
    {
        return context.ReadValue<float>() == 1 ? true : false;
    }

    private bool GetButtonStatus(InputAction.CallbackContext context, Action eventTrigger)
    {
        bool isPressed = context.ReadValue<float>() == 1 ? true : false;

        if(isPressed)
            eventTrigger?.Invoke();

        return isPressed;
    }

    private bool GetCompositeStatus(InputAction.CallbackContext context)
    {
        return context.ReadValue<Vector2>() != Vector2.zero ? true : false;
    }

    private bool GetCompositeStatus(InputAction.CallbackContext context, out Vector2 vector)
    {
        bool isPressed = context.ReadValue<Vector2>() != Vector2.zero ? true : false;
        vector = context.ReadValue<Vector2>();
        return isPressed;
    }

    private bool GetCompositeStatus(InputAction.CallbackContext context, Action<Vector2> eventTrigger, out Vector2 vector)
    {
        bool isPressed = context.ReadValue<Vector2>() != Vector2.zero ? true : false;
        vector = context.ReadValue<Vector2>();

        if(isPressed)
            eventTrigger?.Invoke(vector);

        return isPressed;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad && change != InputDeviceChange.Removed)
        {
            usingController = true;
            //Debug.Log($"Using Controller");
        }
        else
        {
            usingController = false;
            //Debug.Log($"Using Keyboard");
        }
    }
}