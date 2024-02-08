using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Dialogue, Overview, Playing, Paused
    }
    
    public GameState State { get => _state; set => ChangeGameState(value); }
    
    private GameState _state;
    private int _dialogueIndex;

    private void Start()
    {
        State = GameState.Dialogue;
        _dialogueIndex = 0;
    }


    private void ChangeGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.Dialogue:
                DialogueUI.Instance.StartDialogue(_dialogueIndex++, OnDialogueFinished);
                break;
        }
    }

    private void OnDialogueFinished()
    {
        State = GameState.Playing;
        Debug.Log("jugando");
    }
}


