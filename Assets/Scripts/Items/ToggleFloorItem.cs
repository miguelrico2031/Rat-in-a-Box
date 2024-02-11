using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ToggleFloorItem : MonoBehaviour
{
    [SerializeField] private float _waitUntilUpdatePath;
    [SerializeField] private List<ToggleDoor> _doorList = new List<ToggleDoor>();

    [SerializeField] private bool _isActive;
    [SerializeField] private List<GameObject> _sprites;

    private void Start()
    {
        if (!_isActive) return;

        _sprites[0].SetActive(false);
        _sprites[1].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rat = other.GetComponentInParent<RatController>();
        if (!rat) return;

        ToggleDoors();
        if (_isActive)
        {
            _sprites[1].SetActive(false);
            _sprites[0].SetActive(true);
            _isActive = false;
        }
        else
        {
            _sprites[0].SetActive(false);
            _sprites[1].SetActive(true);
            _isActive = true;
        }
        
    }

    private void ToggleDoors()
    {

        
        foreach (ToggleDoor door in _doorList)
        {
            door.Toggle();
        }

        Invoke("UpdatePath", _waitUntilUpdatePath);
    }

    private void UpdatePath()
    {
        //AstarPath.active.Scan();
    }

}
