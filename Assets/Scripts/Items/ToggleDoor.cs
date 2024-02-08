using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDoor : MonoBehaviour
{
    [SerializeField] private bool _isOpen;

    private PolygonCollider2D _col;

    private void Start()
    {
        _col = GetComponent<PolygonCollider2D>();
        if(_isOpen)
        {
            _col.enabled = false;
        }
    }

    public void Toggle()
    {
        if (_isOpen) Close(); else Open();
    }

    void Open()
    {
        Debug.Log("abierto");
        _col.enabled = false;
        _isOpen = true;
    }

    void Close()
    {
        Debug.Log("cerrado");
        _col.enabled = true;
        _isOpen = false;
    }
}
