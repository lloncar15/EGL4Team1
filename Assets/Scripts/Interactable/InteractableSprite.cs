using UnityEngine;

public abstract class InteractableSprite : MonoBehaviour, IInteractable {
    private bool _isInRange;

    protected void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;
        
        _isInRange = true;
        
        if (!CanBeInteracted())
            return;
        
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        controller.OnInteractionZoneEnter(this);
    }
    
    protected void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        _isInRange = false;
        PlayerInteractionController controller = other.GetComponent<PlayerInteractionController>();
        controller.OnInteractionZoneExit();
    }

    public abstract void Interact();
    
    /// <summary>
    /// Checks if the object can be interacted with.
    /// </summary>
    /// <returns>True if in Painting state and within trigger range</returns>
    public virtual bool CanBeInteracted() {
        return _isInRange;
    }
}