
using System;
using UnityEngine;

public abstract class AItem : MonoBehaviour
{
    public ItemInfo Info { get => _info; }

    [SerializeField] private ItemInfo _info;

    private float _rangeSquared;
    
    private void Awake()
    {
        _rangeSquared = Info.Range * Info.Range;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Info.Range);
    }

    public virtual bool IsInRange(Transform t)
    {
        return (t.position - transform.position).sqrMagnitude <= _rangeSquared;
    }

    public abstract Vector2 GetTarget(Vector3 ratPosition);


    private void OnTriggerEnter2D(Collider2D other)
    {
        var rat = other.GetComponentInParent<RatController>();
        if (!rat) return;
        InteractWithRat(rat);
    }

    protected virtual void InteractWithRat(RatController ratController)
    {
        ratController.OnItemCollision(this);
    }
    
    
}
