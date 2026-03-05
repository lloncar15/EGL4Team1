using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : PersistentSingleton<InputController> {
    private PlayerInputActions _inputActions;
    
    public Vector2 MoveInput {get; private set;}

    public static event Action OnDash;
    
    protected override void Awake() {
        _inputActions = new PlayerInputActions();
        DisableCursor();
        base.Awake();
    }
    
    private void OnEnable() {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();

        _inputActions.Player.Dash.performed += OnDashPerformed;
    }

    private void OnDisable() {
        if (_inputActions == null) 
            return;
        
        _inputActions.Player.Dash.performed -= OnDashPerformed;
        
        _inputActions.Disable();
    }

    private void Update() {
        ReadMoveInputs();
    }
    
    private void OnDashPerformed(InputAction.CallbackContext ctx) {
        OnDash?.Invoke();
    }

    /// <summary>
    /// Reads current move input values from the input action asset.
    /// </summary>
    private void ReadMoveInputs() {
        MoveInput = _inputActions.Player.Move.ReadValue<Vector2>();
    }

    /// <summary>
    /// Locks and hides the cursor.
    /// </summary>
    private static void EnableCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Unlocks and shows the cursor.
    /// </summary>
    private static void DisableCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}