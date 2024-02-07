using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

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
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        
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
        ItemManager.Instance.RemoveItem(selectable.GetComponentInParent<AItem>());
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
