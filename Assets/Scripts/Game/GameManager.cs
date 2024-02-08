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
    public event Action<int> TimerDecreased;
    
    public Level CurrentLevel { get; private set; }

    [SerializeField] private Level[] _levels;
    
    private GameState _state;
    private int _dialogueIndex;

    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _state = GameState.None;
        SceneManager.sceneLoaded += OnSceneStart;

        foreach (var level in _levels)
        {
            if (level.Scene != SceneManager.GetActiveScene().name) continue;
        
            CurrentLevel = level;
            break;
        }
    }
    
    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        State = GameState.Dialogue;
    }

    private IEnumerator Start()
    {
        yield return null;
        _dialogueIndex = 0;
        //State = GameState.Dialogue;
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
            
            case GameState.Playing:
                StartCoroutine(LevelCountDown());
                break;
        }
        
        GameStateChange?.Invoke(_state);
        
    }

    private IEnumerator LevelCountDown()
    {
        int remainingTime = CurrentLevel.LevelTime;
        TimerDecreased?.Invoke(remainingTime);
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1);
            remainingTime--;
            TimerDecreased?.Invoke(remainingTime);
        }
        
        Debug.Log("gameover rata eletrocuta");
    }

    private void OnDialogueFinished()
    {
        State = GameState.Overview;
    }
}


