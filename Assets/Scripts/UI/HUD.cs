using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] private GameObject _itemsUI;
    [SerializeField] private GameObject _removeButton;
    [SerializeField] private GameObject _cancelButton;
    

    private void Awake()
    {
        if (Instance) Destroy(gameObject);
        Instance = this;
        SceneManager.sceneLoaded += OnSceneStart;
    }

    private void Start()
    {
        SelectionManager.Instance.SelectedChange += OnSelectedChange;
        PlacementManager.Instance.ItemChange += OnItemChange;
        _removeButton.SetActive(false);
        _cancelButton.SetActive(false);
        GameManager.Instance.GameStateChange += OnGameStateChange;
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        _removeButton.SetActive(false);
        _cancelButton.SetActive(false);
        _itemsUI.SetActive(false);
    }

    private void OnGameStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.Dialogue:
                _itemsUI.SetActive(false);
                break;
            case GameManager.GameState.Overview:
                _itemsUI.SetActive(true);
                break;
        }
    }

    public void SelectItem(ItemInfo item)
    {
        PlacementManager.Instance.SetItemToPlace(item);
    }

    public void CancelItem()
    {
        PlacementManager.Instance.CancelItemToPlace();
    }

    public void RemoveItem()
    {
        var selectable = SelectionManager.Instance.Selected;
        if (!selectable) return;

        var nonLidItem = selectable.GetComponentInParent<AItem>();

        if (nonLidItem) ItemManager.Instance.RemoveItem(nonLidItem);
        else ItemManager.Instance.RemoveLid(selectable.transform.parent.gameObject);
    }

    private void OnSelectedChange()
    {
        _removeButton.SetActive(SelectionManager.Instance.Selected != null);
    }

    private void OnItemChange()
    {
        _cancelButton.SetActive(PlacementManager.Instance.ItemToPlace != null);
    }
}
