using System;
using UnityEngine;

public class PlayerAbilityController : MonoBehaviour {
    [SerializeField] private LayerMask visitorLayer;
    [SerializeField] private GameStateController gameStateController;
    [SerializeField] private PlayerMovementController move;

    [SerializeField] private float abilityRange = 3f;
    [SerializeField] private float targetingAngle = 0.6f;

    private const int MAX_COLLIDERS = 10;
    private Collider[] _hitColliders;

    private void Awake() {
        _hitColliders = new Collider[MAX_COLLIDERS];
    }

    private void OnEnable() {
        InputController.OnItemSelected += KeyPressed;
    }

    private void OnDisable() {
        InputController.OnItemSelected -= KeyPressed;
    }

    private void KeyPressed(int key) {
        if (gameStateController.currentGameState != GameState.Opened)
            return;

        if (key == 0) {
            TryMesmerize();
        }
        else {
            TryDrain();
        }
    }

    private void TryMesmerize() {
        Visitor visitor = GetClosestVisitorInFront();
        
        if (visitor)
            visitor.Mesmerize();
    }

    private void TryDrain() {
        Visitor visitor = GetClosestVisitorInFront();
        
        if (visitor)
            visitor.Drain();
    }

    private Visitor GetClosestVisitorInFront() {
        int hits = Physics.OverlapSphereNonAlloc(transform.position, abilityRange, _hitColliders, visitorLayer);

        if (hits <= 0)
            return null;
        
        Visitor closestVisitor = null;
        float closestDistance = Mathf.Infinity;

        Vector3 forward = move.FacingDirection;

        for (int i = 0; i < hits; i++) {
            Visitor hit = _hitColliders[i].GetComponent<Visitor>();
            if (!hit)
                continue;
            
            Vector3 directionToVisitor = (hit.transform.position - transform.position).normalized;
            float dot = Vector3.Dot(forward, directionToVisitor);
            if (dot < targetingAngle)
                continue;
            
            float distance = Vector3.Distance(transform.forward, hit.transform.position);

            if (distance < closestDistance) {
                closestDistance = distance;
                closestVisitor = hit;
            }
        }

        return closestVisitor;
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, abilityRange);
    }
#endif
}