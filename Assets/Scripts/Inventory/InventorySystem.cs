using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem :MonoBehaviour {
    [SerializeField] public List<Item> items = new();

    public static event Action<Item> OnItemAdded;
    public static event Action<int> OnItemRemoved;
    public static event Action UpdateItems;

    public List<Item> Items => items;

    private void Start() {
        UpdateItems?.Invoke();
    }

    public void AddItem(Item newItem) {
        items.Add(newItem);
        OnItemAdded?.Invoke(newItem);
    }

    public void RemoveItem(int itemIndex) {
        items.RemoveAt(itemIndex);
        OnItemRemoved?.Invoke(itemIndex);
    }
    
    public Item GetItem(int itemIndex) {
        return items[itemIndex];
    }

    public bool HasItemAtIndex(int index) {
        return index >= 0 && index < items.Count;
    }
}