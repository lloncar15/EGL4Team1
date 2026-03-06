using System.Collections.Generic;
using UnityEngine;

public class BuyingAreaScript : MonoBehaviour {
    [SerializeField] public BuyingAreaUI buyingUI;
    
    public void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        buyingUI.gameObject.SetActive(true);
        buyingUI.Show();
    }

    public void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Player"))
            return;

        buyingUI.Hide();
    }
}