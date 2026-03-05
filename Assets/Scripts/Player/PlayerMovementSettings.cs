using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Game/Player Movement Settings")]
public class PlayerMovementSettings : ScriptableObject {
    [Header("Movement")]
    public float moveSpeed = 6f;

    [Header("Dash")]
    public float dashSpeed = 18f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.6f;
}