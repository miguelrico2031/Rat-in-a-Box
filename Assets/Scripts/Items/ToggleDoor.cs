using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ToggleDoor : MonoBehaviour
{
    [SerializeField] private bool _isOpen;
    [SerializeField] private GameObject _sprite;

    private PolygonCollider2D _col;

    private void Start()
    {
        _col = GetComponent<PolygonCollider2D>();
        if(_isOpen)
        {
            _col.enabled = false;
            AstarPath.active.Scan();
            _sprite.SetActive(false);
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
        _sprite.SetActive(false);
        AstarPath.active.Scan();
        FindObjectOfType<RatController>().RecalcPath();
    }

    void Close()
    {
        Debug.Log("cerrado");
        _col.enabled = true;
        _isOpen = false;
        _sprite.SetActive(true);
        AstarPath.active.Scan();
        FindObjectOfType<RatController>().RecalcPath();
    }
}
