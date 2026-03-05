using System;
using UnityEngine;

public class InteractableArtifactHolder : InteractableSprite {
    [SerializeField] private SpriteRenderer itemImage;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Item item;
    [SerializeField] private InventorySystem inventory;

    private bool _holdsAnArtifact;

    private void Start() {
        if (defaultSprite)
            itemImage.sprite = defaultSprite;
    }

    public override void Interact() {
        if (!_holdsAnArtifact || !CanBeInteracted()) 
            return;
        
        inventory.AddItem(item);
        RemoveItem();
    }

    private void RemoveItem() {
        item = null;
        itemImage.sprite = defaultSprite;
        _holdsAnArtifact = false;
    }

    private void AddItem(Item newItem) {
        item = newItem;
        itemImage.sprite = item.icon;
    }

    public void TryAndPlaceItem(int index) {
        if (!inventory.HasItemAtIndex(index))
            return;
        
        if (_holdsAnArtifact || !CanBeInteracted())
            return;

        AddItem(inventory.GetItem(index));
        inventory.RemoveItem(index);
        _holdsAnArtifact = true;
    }
}