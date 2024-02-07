using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private Collider2D _collider;
    private Material _spriteMaterial;
    private int _outlineEnabledID;
    
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _outlineEnabledID = Shader.PropertyToID("_OutlineEnabled");
        _spriteMaterial = transform.parent.Find("Sprite").GetComponent<SpriteRenderer>().material;
        _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlacementManager.Instance.ItemToPlace) return;
        _spriteMaterial.SetFloat(_outlineEnabledID, 1f);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (PlacementManager.Instance.ItemToPlace) return;
        SelectionManager.Instance.Selected = this;
    }

    public void ToggleLid(bool on)
    {
        _collider.enabled = !on;
    }
}
