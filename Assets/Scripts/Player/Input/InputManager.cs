using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Input Variables
    private Vector2 moveInput;
    private bool primaryInput;
    private bool secondaryInput;
    #endregion

    #region Control Schemes
    [SerializeField] private ControlScheme controlScheme;

    private enum ControlScheme
    {
        DefaultKeyboard,
        InvertedKeyboard,
        DefaultController,
        InvertedController
    }
    #endregion

    private static PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Update()
    {
        moveInput = GetMoveInput();
        primaryInput = GetPrimaryInput();
        secondaryInput = GetSecondaryInput();
    }

    public static Vector2 GetMoveInput()
    {
        return playerInput.Ground.Walk.ReadValue<Vector2>();
    }

    public static bool GetPrimaryInput()
    {
        return playerInput.Ground.UsePrimary.ReadValue<float>() == 1 ? true : false;
    }

    public static bool GetSecondaryInput()
    {
        return playerInput.Ground.UseSecondary.ReadValue<float>() == 1 ? true : false;
    }

    public static Vector2 GetMousePosition()
    {
        return playerInput.Ground.CursorPosition.ReadValue<Vector2>();
    }
}
