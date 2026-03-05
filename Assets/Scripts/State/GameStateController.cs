using System;
using UnityEngine;
#pragma warning disable CS0414 // Field is assigned but its value is never used

public class GameStateController : MonoBehaviour {
    [SerializeField] private GameState currentGameState = GameState.Placement;

    public static event Action MuseumOpened;
    
    private void OnEnable() {
        InputController.ChangeMode += ChangeMode;
    }

    private void OnDisable() {
        InputController.ChangeMode -= ChangeMode;
    }

    private void ChangeMode() {
        currentGameState = GameState.Placement;
        MuseumOpened?.Invoke();
        InputController.ChangeMode -= ChangeMode;
    }
}

public enum GameState {
    Placement,
    Opened
}