using System;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour {
    private static readonly int Attack = Animator.StringToHash("Attack");
    public static event Action OnInteractionZoneEntered;
    public static event Action OnInteractionZoneExited;
    
    public Animator animator;
    
    private IInteractable _interactableInRange;
    private bool _canInteractWithItems = true;

    private void OnEnable() {
        InputController.OnInteract += Interact;
        InputController.OnItemSelected += OnKeyPressed;
        GameStateController.MuseumOpened += OnMuseumOpened;
    }

    private void OnDisable() {
        InputController.OnInteract -= Interact;
        InputController.OnItemSelected -= OnKeyPressed;
        GameStateController.MuseumOpened -= OnMuseumOpened;
    }

    /// <summary>
    /// Called when the player enters an interaction zone.
    /// Stores the interactable reference and fires the zone entered event for UI.
    /// </summary>
    /// <param name="interactable">The interactable object in range</param>
    public void OnInteractionZoneEnter(IInteractable interactable) {
        _interactableInRange = interactable;
        OnInteractionZoneEntered?.Invoke();
    }

    /// <summary>
    /// Called when the player exits an interaction zone.
    /// Clears the interactable reference and fires the zone exited event for UI.
    /// </summary>
    public void OnInteractionZoneExit() {
        _interactableInRange = null;
        OnInteractionZoneExited?.Invoke();
    }

    private void OnMuseumOpened() {
        _canInteractWithItems = false;
    }

    /// <summary>
    /// Attempts to interact with the current interactable in range.
    /// Delegates all state and availability checks to the interactable itself via CanBeInteracted().
    /// </summary>
    private void Interact() {
        animator.SetTrigger(Attack);
        
        if (!_canInteractWithItems)
            return;
        
        if (_interactableInRange == null)
            return;

        if (!_interactableInRange.CanBeInteracted())
            return;
        
        _interactableInRange.Interact();
    }

    private void OnKeyPressed(int index) {
        animator.SetTrigger(Attack);
        
        if (!_canInteractWithItems)
            return;
        
        if (_interactableInRange == null)
            return;

        InteractableArtifactHolder holder = (InteractableArtifactHolder)_interactableInRange;
        if (!holder)
            return;

        holder.TryAndPlaceItem(index);
    }
}