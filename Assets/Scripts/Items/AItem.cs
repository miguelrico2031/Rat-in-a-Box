
using System;
using UnityEngine;

public abstract class AItem : MonoBehaviour
{
    public ItemInfo Info { get => _info; }
    public bool IsCovered { get; set; }

    [SerializeField] private ItemInfo _info;

    //private float _rangeSquared;
    private PolygonCollider2D _rangeCollider;
    
    protected virtual void Awake()
    {
        //_rangeSquared = Info.Range * Info.Range;
        _rangeCollider = GetComponentInChildren<PolygonCollider2D>();
        _rangeCollider.enabled = false;
    }

    public bool IsInRange(Transform t)
    {
        if (IsCovered) return false;
        _rangeCollider.enabled = true;
        var overlap = _rangeCollider.OverlapPoint(t.position);
        _rangeCollider.enabled = false;
        return overlap;
    }

    public abstract Vector2 GetDestination(Vector3 ratPosition);

    public virtual void OnSetAsTarget()
    {
        
    }

    public virtual void OnUnsetAsTarget()
    {
        
    }

    
    
}
