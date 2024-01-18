using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void GameStateChangeHandler(GameState newState);
    public static event GameStateChangeHandler OnGameStateChange;

    private static GameState currentGameState;
    // Enum to represent the game states
    public enum GameState
    {
        BEGIN,
        PAUSE,
        CONTINUE,
        GAMEOVER,
        CAPTURE,
        CAPTURECOMPLETE
    }

    // Method to trigger the event and change the game state
    public static void SetGameState(GameState newState)
    {
        if (OnGameStateChange != null)
        {
            OnGameStateChange(newState);
        }
    }

    public static GameState GetGameState
    {
        get { return currentGameState; }

    }
}