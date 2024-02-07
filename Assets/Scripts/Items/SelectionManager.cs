using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    public Selectable Selected { get => _selectable; set => SetSelected(value); }

    public event Action SelectedChange;

    private Selectable _selectable;
    
    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;

        SceneManager.sceneLoaded += OnSceneStart;
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        Selected = null;
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || PlacementManager.Instance.ItemToPlace) return;
        
        Vector2 pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

        var col = Physics2D.OverlapCircle(pos, .05f);

        if (!col || !col.TryGetComponent<Selectable>(out var s)) Selected = null;

    }

    private void SetSelected(Selectable s)
    {
        _selectable = s;
        
        SelectedChange?.Invoke();
    }
}