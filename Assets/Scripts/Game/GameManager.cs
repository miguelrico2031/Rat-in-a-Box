using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Dialogue, Overview, Playing, Paused, None
    }
    
    public static GameManager Instance { get; private set; }
    public GameState State { get => _state; set => ChangeGameState(value); }
    public event Action<GameState> GameStateChange;
    
    private GameState _state;
    private int _dialogueIndex;

    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _state = GameState.None;
        SceneManager.sceneLoaded += OnSceneStart;
    }
    
    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        State = GameState.Dialogue;
    }

    private void Start()
    {
        _dialogueIndex = 0;
        State = GameState.Dialogue;
    }

    private void ChangeGameState(GameState newState)
    {
        if (newState == _state) return;
        _state = newState;
        switch (newState)
        {
            case GameState.Dialogue:
                DialogueUI.Instance.StartDialogue(_dialogueIndex++, OnDialogueFinished);
                break;
        }
        
        GameStateChange?.Invoke(_state);
        
    }

    private void OnDialogueFinished()
    {
        State = GameState.Overview;
    }
}


