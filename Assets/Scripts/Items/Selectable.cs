using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private bool _onlyHoverable;
    
    private Collider2D _collider;
    private Material _spriteMaterial;
    private SpriteRenderer _range, _spriteRenderer;
    private int _outlineEnabledID, _outlineColorID;
    private int _spriteOrder;
    private bool _selected;
    
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _outlineEnabledID = Shader.PropertyToID("_OutlineEnabled");
        _outlineColorID = Shader.PropertyToID("_SolidOutline");
        _spriteRenderer = transform.parent.Find("Sprite").GetComponent<SpriteRenderer>();
        _spriteMaterial = _spriteRenderer.material;
        _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
        _range = transform.parent.Find("Range").GetComponent<SpriteRenderer>();
        _spriteOrder = _spriteRenderer.sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlacementManager.Instance.ItemToPlace) return;
        _spriteMaterial.SetFloat(_outlineEnabledID, 1f);
        _range.enabled = true;
        _spriteRenderer.sortingOrder = 1000;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_selected) _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
        _range.enabled = false;
        _spriteRenderer.sortingOrder = _spriteOrder;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_onlyHoverable || PlacementManager.Instance.ItemToPlace) return;
        SelectionManager.Instance.Selected = this;
        _spriteMaterial.SetColor(_outlineColorID, new Color(0.851f, 0.341f, 0.388f));
        _selected = true;
    }

    public void OnDeselected()
    {
        _spriteMaterial.SetColor(_outlineColorID, Color.white);
        _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
        _selected = false;
    }

    public void ToggleLid(bool on)
    {
        _collider.enabled = !on;

        // ANTON sonido tapa
        //Debug.Log("tapa");
        MusicManager.Instance.PlaySound("tapa");
    }
}
