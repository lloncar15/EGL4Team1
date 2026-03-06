using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : GenericSingleton<ResourceManager> {
    [SerializeField] public int money;
    [SerializeField] public List<InteractableArtifactHolder> artifacts;

    public static event Action MoneyChanged;

    private int _moneyModifier;
    
    private void OnEnable() {
        Visitor.TicketPaid += AddMoney;
        GameStateController.MuseumOpened += CalculateMoneyModifier;
    }

    private void OnDisable() {
        Visitor.TicketPaid -= AddMoney;
        GameStateController.MuseumOpened -= CalculateMoneyModifier;
    }

    private void AddMoney(int amount) {
        money += amount + _moneyModifier;
        MoneyChanged?.Invoke();
    }

    public void RemoveMoney(int amount) {
        money -= amount;
        MoneyChanged?.Invoke();
    }

    private void CalculateMoneyModifier() {
        foreach (InteractableArtifactHolder artifactHolder in artifacts) {
            if (!artifactHolder.HoldsAnArtifact)
                continue;
            
            _moneyModifier += artifactHolder.item.moneyModifier;
        }
    }
}