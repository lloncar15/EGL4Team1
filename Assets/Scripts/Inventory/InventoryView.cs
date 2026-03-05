using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryView : MonoBehaviour {
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private InventoryItemView itemPrefab;
    
    [SerializeField] private List<InventoryItemView> itemViews = new();
    
    private void OnEnable() {
        InventorySystem.OnItemAdded += AddItem;
        InventorySystem.OnItemRemoved += RemoveItem;
        InventorySystem.UpdateItems += UpdateItems;
        GameStateController.MuseumOpened += OnMuseumOpened;
    }

    private void OnDisable() {
        InventorySystem.OnItemAdded -= AddItem;
        InventorySystem.OnItemRemoved -= RemoveItem;
        InventorySystem.UpdateItems -= UpdateItems;
        GameStateController.MuseumOpened -= OnMuseumOpened;
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

    private void OnMuseumOpened() {
        transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => {
                gameObject.SetActive(false);
            });
    }
}