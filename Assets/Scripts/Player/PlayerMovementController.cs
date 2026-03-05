using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private PlayerMovementSettings settings;
    
    [Header("References")]
    [SerializeField] private SpriteRenderer renderer;

    private PlayerInputActions _inputActions;
    private Vector2 _moveInput;

    private Rigidbody _rb;
    private bool _isDashing;
    private bool _dashOnCooldown;
    private Vector2 _lastMoveDirection = Vector2.down;
    
    private WaitForSeconds _waitDashDuration;
    private WaitForSeconds _waitDashCooldown;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _inputActions = new PlayerInputActions();
        
        _waitDashDuration = new WaitForSeconds(settings.dashDuration);
        _waitDashCooldown = new WaitForSeconds(settings.dashCooldown - settings.dashDuration);
    }

    private void OnEnable() {
        _inputActions.Player.Enable();
        _inputActions.Player.Dash.performed += OnDash;
    }

    private void OnDisable() {
        _inputActions.Player.Dash.performed -= OnDash;
        _inputActions.Player.Disable();
    }

    private void Update() {
        UpdateMovement();
        UpdateSpriteDirection();
    }

    private void FixedUpdate() {
        if (_isDashing)
            return;
        
        Vector3 moveVelocity = new Vector3(_moveInput.x, 0, _moveInput.y) * settings.moveSpeed;
        _rb.linearVelocity = moveVelocity;
    }

    #region Movement

    private void UpdateMovement() {
        _moveInput = _inputActions.Player.Move.ReadValue<Vector2>();

        if (_moveInput.sqrMagnitude > 0.01f)
            _lastMoveDirection = _moveInput.normalized;
    }

    #endregion
    
    #region Dash

    private void OnDash(InputAction.CallbackContext ctx) {
        if (_isDashing || _dashOnCooldown)
            return;

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine() {
        _isDashing = true;
        _dashOnCooldown = true;
        
        Vector3 dashDirection = new(_lastMoveDirection.x, 0f, _lastMoveDirection.y);
        _rb.linearVelocity = dashDirection * settings.dashSpeed;

        yield return _waitDashDuration;

        _isDashing = false;
        _rb.linearVelocity = Vector3.zero;

        yield return _waitDashCooldown;
        _dashOnCooldown = false;
    }
    
    #endregion

    #region Sprite

    private void UpdateSpriteDirection() {
        if (!renderer)
            return;
        
        if (_lastMoveDirection.x > 0.01f)
            renderer.flipX = false;
        else if (_lastMoveDirection.x < -0.01f)
            renderer.flipX = true;
    }

    #endregion
}