using System;
using TMPro;
using UnityEngine;

public class ResourceView : MonoBehaviour {
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private TextMeshProUGUI moneyText;

    private void OnEnable() {
        ResourceManager.MoneyChanged += OnMoneyChanged;
    }

    private void OnDisable() {
        ResourceManager.MoneyChanged -= OnMoneyChanged;
    }

    private void Start() {
        UpdateMoneyUI();
    }

    private void OnMoneyChanged() {
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI() {
        moneyText.text = "Coins: " + resourceManager.money;
    }
}