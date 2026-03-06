using System;
using DG.Tweening;
using UnityEngine;

public class BuyingAreaUI : MonoBehaviour {
    public bool HasBought;
    [SerializeField] public ResearchRoom researchRoom;
    
    private void OnDisable() {
        InputController.Buying -= Buy;
    }

    public void Show() {
        if (HasBought)
            return;
        
        transform.localScale =  Vector3.zero;
        transform.DOScale(Vector3.one, 0.4f).OnComplete(() => {
            InputController.Buying += Buy;
        });
    }

    public void Hide() {
        transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => {
            InputController.Buying -= Buy;
        });
    }

    private void Buy() {
        HasBought = true;
        ResourceManager.Instance.RemoveMoney(500);
        researchRoom.gameObject.SetActive(true);
        researchRoom.AnimateEnter();
        Hide();
    }
}