using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private PlayerMovementSettings settings;

    [Header("References")] 
    [SerializeField] private TrailRenderer dashTrail;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 _moveInput;

    private Rigidbody _rb;
    private bool _isDashing;
    private bool _dashOnCooldown;
    private Vector2 _lastMoveDirection = Vector2.down;
    
    private WaitForSeconds _waitDashDuration;
    private WaitForSeconds _waitDashCooldown;

    public Vector3 FacingDirection => new Vector3(_lastMoveDirection.x, 0, _lastMoveDirection.y);
    
    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        
        _waitDashDuration = new WaitForSeconds(settings.dashDuration);
        _waitDashCooldown = new WaitForSeconds(settings.dashCooldown - settings.dashDuration);
    }

    private void OnEnable() {
        InputController.OnDash += OnDash;
    }

    private void OnDisable() {
        InputController.OnDash -= OnDash;
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
        _moveInput = InputController.Instance.MoveInput;

        if (_moveInput.sqrMagnitude > 0.01f)
            _lastMoveDirection = _moveInput.normalized;
    }

    #endregion
    
    #region Dash

    private void OnDash() {
        if (_isDashing || _dashOnCooldown)
            return;

        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine() {
        _isDashing = true;
        _dashOnCooldown = true;
        dashTrail.emitting = true;
        
        Vector3 dashDirection = new(_lastMoveDirection.x, 0f, _lastMoveDirection.y);
        _rb.linearVelocity = dashDirection * settings.dashSpeed;

        yield return _waitDashDuration;

        _isDashing = false;
        dashTrail.emitting = false;
        _rb.linearVelocity = Vector3.zero;

        yield return _waitDashCooldown;
        _dashOnCooldown = false;
    }
    
    #endregion

    #region Sprite

    private void UpdateSpriteDirection() {
        if (!spriteRenderer)
            return;
        
        if (_lastMoveDirection.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (_lastMoveDirection.x < -0.01f)
            spriteRenderer.flipX = true;
    }

    #endregion
}