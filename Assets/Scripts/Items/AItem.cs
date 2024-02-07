
using System;
using UnityEngine;

public abstract class AItem : MonoBehaviour
{
    public ItemInfo Info { get => _info; }
    public bool IsCovered { get; private set; }

    [SerializeField] private ItemInfo _info;

    protected Collider2D _collider;
    private PolygonCollider2D _rangeCollider;
    private Selectable _selectable;
    
    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rangeCollider = GetComponentInChildren<PolygonCollider2D>();
        _rangeCollider.enabled = false;
        _selectable = GetComponentInChildren<Selectable>();
    }

    public bool IsInRange(Transform t)
    {
        if (IsCovered) return false;
        _rangeCollider.enabled = true;
        var overlap = _rangeCollider.OverlapPoint(t.position);
        _rangeCollider.enabled = false;
        return overlap;
    }

    public void ToggleLid(bool on)
    {
        IsCovered = on;
        _collider.enabled = !on;
        
        if(_selectable) _selectable.ToggleLid(on);
    }

    public abstract Vector2 GetDestination(Vector3 ratPosition);

    public virtual void OnSetAsTarget()
    {
        
    }

    public virtual void OnUnsetAsTarget()
    {
        
    }

    
    
}
