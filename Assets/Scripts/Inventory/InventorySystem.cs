using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem :MonoBehaviour {
    [SerializeField] public List<Artifact> items = new();

    public static event Action<Artifact> OnItemAdded;
    public static event Action<int> OnItemRemoved;
    public static event Action UpdateItems;

    public List<Artifact> Items => items;

    private void Start() {
        UpdateItems?.Invoke();
    }

    public void AddItem(Artifact newItem) {
        items.Add(newItem);
        OnItemAdded?.Invoke(newItem);
    }

    public void RemoveItem(int itemIndex) {
        items.RemoveAt(itemIndex);
        OnItemRemoved?.Invoke(itemIndex);
    }
    
    public Artifact GetItem(int itemIndex) {
        return items[itemIndex];
    }

    public bool HasItemAtIndex(int index) {
        return index >= 0 && index < items.Count;
    }
}