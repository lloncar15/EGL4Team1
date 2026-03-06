using System.Collections.Generic;
using UnityEngine;

public class InteractableArtifactHolder : InteractableSprite {
    [SerializeField] private SpriteRenderer itemImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] public Artifact item;
    [SerializeField] private InventorySystem inventory;
    [SerializeField] public List<Transform> standingTransforms;
    [SerializeField] public bool isMainArtifact;

    private bool _holdsAnArtifact;
    public bool HoldsAnArtifact => _holdsAnArtifact;

    private void Start() {
        if (isMainArtifact) {
            _holdsAnArtifact = true;
            return;
        }

        if (item) {
            _holdsAnArtifact = true;
            itemImage.sprite = item.icon;
            itemImage.color = item.color;
            return;
        }
        
        if (defaultSprite) {
            itemImage.sprite = defaultSprite;
            itemImage.color = Color.black;
        }
    }

    public override void Interact() {
        if (!_holdsAnArtifact || !CanBeInteracted() || isMainArtifact) 
            return;
        
        inventory.AddItem(item);
        RemoveItem();
    }

    private void RemoveItem() {
        item = null;
        itemImage.sprite = defaultSprite;
        itemImage.color = Color.black;
        _holdsAnArtifact = false;
    }

    private void AddItem(Artifact newItem) {
        item = newItem;
        itemImage.sprite = item.icon;
        itemImage.color = newItem.color;
    }

    public void TryAndPlaceItem(int index) {
        if (!inventory.HasItemAtIndex(index))
            return;
        
        if (_holdsAnArtifact || !CanBeInteracted() || isMainArtifact)
            return;

        AddItem(inventory.GetItem(index));
        inventory.RemoveItem(index);
        _holdsAnArtifact = true;
    }
}