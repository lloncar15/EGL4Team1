using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour {
    [SerializeField] public Image itemImage;

    public void Initialize(Item item) {
        itemImage.sprite = item.icon;
    }
}