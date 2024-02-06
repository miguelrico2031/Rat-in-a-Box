
using System;
using UnityEngine;

public abstract class AItem : MonoBehaviour
{
    public ItemInfo Info { get => _info; }

    [SerializeField] private ItemInfo _info;

    //private float _rangeSquared;
    private PolygonCollider2D _rangeCollider;
    
    protected virtual void Awake()
    {
        //_rangeSquared = Info.Range * Info.Range;
        _rangeCollider = GetComponentInChildren<PolygonCollider2D>();
        _rangeCollider.enabled = false;
    }

    public virtual bool IsInRange(Transform t)
    {
        //return (t.position - transform.position).sqrMagnitude <= _rangeSquared;
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
