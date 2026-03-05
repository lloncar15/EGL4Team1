using UnityEngine;

public abstract class Item : ScriptableObject {
    [Header("Base data")]
    public string itemName;
    public Sprite icon;
    public string itemDescription;
}