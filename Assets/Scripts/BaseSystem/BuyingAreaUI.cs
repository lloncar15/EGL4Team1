using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BuyingAreaUI : MonoBehaviour {
    public bool HasBought;
    [SerializeField] public ResearchRoom researchRoom;
    [SerializeField] public GameObject researchUI;
    [SerializeField] public GameObject vampiricTouch;

    private void OnEnable() {
        InputController.BuyingResearch += BuyResearch;
    }

    private void OnDisable() {
        InputController.Buying -= Buy;
        InputController.BuyingResearch -= BuyResearch;
    }

    public void Show() {
        if (HasBought) {
            ShowResearchUI();
            return;
        }
            
        transform.localScale =  Vector3.zero;
        transform.DOScale(Vector3.one, 0.4f).OnComplete(() => {
            InputController.Buying += Buy;
        });
    }

    public void Hide() {
        if (HasBought) {
            HideResearchUI();
        }
        else {
            transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => {
                InputController.Buying -= Buy;
            });
        }
    }

    private void ShowResearchUI() {
        researchUI.SetActive(true);
        researchUI.transform.localScale =  Vector3.zero;
        researchUI.transform.DOScale(Vector3.one, 0.4f).OnComplete(() => {
            InputController.Buying += Buy;
        });
    }

    private void HideResearchUI() {
        researchUI.transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => {
            InputController.Buying -= Buy;
        });
    }

    private void BuyResearch() {
        ResourceManager.Instance.RemoveMoney(300);
        vampiricTouch.transform.DOPunchScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f, 0, 1f);
    }

    private void Buy() {
        HasBought = true;
        ResourceManager.Instance.RemoveMoney(500);
        researchRoom.gameObject.SetActive(true);
        researchRoom.AnimateEnter();
        
        
        transform.DOScale(Vector3.zero, 0.4f).OnComplete(() => {
            InputController.Buying -= Buy;
        });
    }
}