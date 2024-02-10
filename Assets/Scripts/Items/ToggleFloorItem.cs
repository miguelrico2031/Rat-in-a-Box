using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ToggleFloorItem : MonoBehaviour
{
    [SerializeField] private float _waitUntilUpdatePath;
    [SerializeField] private List<ToggleDoor> _doorList = new List<ToggleDoor>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rat = other.GetComponentInParent<RatController>();
        if (!rat) return;

        ToggleDoors();
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
