using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MuseumOpenView : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private string openedString;

    private void OnEnable() {
        GameStateController.MuseumOpened += OnMuseumOpened;
    }

    private void OnDisable() {
        GameStateController.MuseumOpened -= OnMuseumOpened;
    }

    private void OnMuseumOpened() {
        title.text = openedString;
        GameStateController.MuseumOpened -= OnMuseumOpened;
    }
}