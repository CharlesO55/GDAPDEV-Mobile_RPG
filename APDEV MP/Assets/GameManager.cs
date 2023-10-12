using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private GameState _gameState;
    public EventHandler SwitchGameStateDelegate;


    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        this._gameState = GameState.START_MENU;



        // Systems/Managers
        SaveSystem.Init();
    }

    public void UpdateGameState(GameState EState)
    {
        this._gameState = EState;
        
        switch (EState)
        {
            case GameState.MOVE_AND_INTERACT:
                break;
            case GameState.DIALOGUE:
                break;
            default:
                Debug.LogWarning("Unknown GameState");
                break;
        }
    }

    public enum GameState
    {
        START_MENU,
        MOVE_AND_INTERACT,
        DIALOGUE
    }
}