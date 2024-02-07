using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] private GameObject _removeButton;
    
    private void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        SelectionManager.Instance.SelectedChange += OnSelectedChange;
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
}
