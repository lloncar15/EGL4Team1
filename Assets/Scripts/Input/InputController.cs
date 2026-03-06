using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : PersistentSingleton<InputController> {
    private PlayerInputActions _inputActions;
    
    public Vector2 MoveInput {get; private set;}

    public static event Action OnDash;
    public static event Action OnInteract;
    public static event Action<int> OnItemSelected;
    public static event Action ChangeMode;

    public static event Action BuyingResearch;

    public static event Action Buying;
    
    private Action<InputAction.CallbackContext>[] _slotCallbacks;
    
    protected override void Awake() {
        _inputActions = new PlayerInputActions();
        DisableCursor();
        base.Awake();
    }
    
    private void OnEnable() {
        _inputActions.UI.Enable();
        _inputActions.Player.Enable();

        _inputActions.Player.Interact.performed += OnInteractPerformed;
        _inputActions.Player.Dash.performed += OnDashPerformed;
        _inputActions.Player.Mode.performed += OnModePerformed;
        _inputActions.Player.Buy.performed += OnBuying;
        _inputActions.Player.BuyingResearch.performed += OnRes;
        
        var slotActions = new[] {
            _inputActions.Player.SelectSlot1,
            _inputActions.Player.SelectSlot2,
            _inputActions.Player.SelectSlot3,
            _inputActions.Player.SelectSlot4,
            _inputActions.Player.SelectSlot5,
            _inputActions.Player.SelectSlot6,
        };

        _slotCallbacks = new Action<InputAction.CallbackContext>[slotActions.Length];
        for (int i = 0; i < slotActions.Length; i++) {
            int slot = i;
            _slotCallbacks[i] = _ => OnItemSelected?.Invoke(slot);
            slotActions[i].performed += _slotCallbacks[i];
        }
    }

    private void OnDisable() {
        if (_inputActions == null) 
            return;
        
        _inputActions.Player.Interact.performed -= OnInteractPerformed;
        _inputActions.Player.Dash.performed -= OnDashPerformed;
        _inputActions.Player.Mode.performed -= OnModePerformed;
        _inputActions.Player.Buy.performed -= OnBuying;
        _inputActions.Player.BuyingResearch.performed -= OnRes;
        
        var slotActions = new[] {
            _inputActions.Player.SelectSlot1,
            _inputActions.Player.SelectSlot2,
            _inputActions.Player.SelectSlot3,
            _inputActions.Player.SelectSlot4,
            _inputActions.Player.SelectSlot5,
            _inputActions.Player.SelectSlot6,
        };

        for (int i = 0; i < slotActions.Length; i++)
            slotActions[i].performed -= _slotCallbacks[i];
        
        _inputActions.Disable();
    }

    private void Update() {
        ReadMoveInputs();
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx) {
        OnInteract?.Invoke();
    }
    
    private void OnDashPerformed(InputAction.CallbackContext ctx) {
        OnDash?.Invoke();
    }

    private void OnModePerformed(InputAction.CallbackContext ctx) {
        ChangeMode?.Invoke();
    }

    private void OnBuying(InputAction.CallbackContext ctx) {
        Buying?.Invoke();
    }

    private void OnRes(InputAction.CallbackContext ctx) {
        BuyingResearch?.Invoke();
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