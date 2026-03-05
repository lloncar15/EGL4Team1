using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour {
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private InventoryItemView itemPrefab;
    
    [SerializeField] private List<InventoryItemView> itemViews = new();
    
    private void OnEnable() {
        InventorySystem.OnItemAdded += AddItem;
        InventorySystem.OnItemRemoved += RemoveItem;
        InventorySystem.UpdateItems += UpdateItems;
    }

    private void OnDisable() {
        InventorySystem.OnItemAdded -= AddItem;
        InventorySystem.OnItemRemoved -= RemoveItem;
        InventorySystem.UpdateItems -= UpdateItems;
    }

    private void UpdateItems() {
        itemViews.Clear();
        foreach (Item item in inventorySystem.Items) {
            InventoryItemView view = Instantiate(itemPrefab, transform, true);
            view.Initialize(item);
            itemViews.Add(view);
        }
    }

    private void AddItem(Item item) {
        InventoryItemView view = Instantiate(itemPrefab, transform, true);
        view.Initialize(item);
        itemViews.Add(view);
    }

    private void RemoveItem(int itemIndex) {
        InventoryItemView view = itemViews[itemIndex];
        itemViews.RemoveAt(itemIndex);
        Destroy(view.gameObject);
    }
}