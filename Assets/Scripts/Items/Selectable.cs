using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    
    private Material _spriteMaterial;
    private int _outlineEnabledID;
    
    private void Awake()
    {
        _outlineEnabledID = Shader.PropertyToID("_OutlineEnabled");
        _spriteMaterial = transform.parent.Find("Sprite").GetComponent<SpriteRenderer>().material;
        _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _spriteMaterial.SetFloat(_outlineEnabledID, 1f);
        Debug.Log("hola");
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        _spriteMaterial.SetFloat(_outlineEnabledID, 0f);
        
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        SelectionManager.Instance.Selected = this;
    }
}
