using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

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
            AstarPath.active.Scan();
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
        AstarPath.active.Scan();
        FindObjectOfType<RatController>().RecalcPath();
    }

    void Close()
    {
        Debug.Log("cerrado");
        _col.enabled = true;
        _isOpen = false;
        AstarPath.active.Scan();
        FindObjectOfType<RatController>().RecalcPath();
    }
}
