using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepulsionItem : AItem
{

    private Collider2D _collider;
    private Vector2 _defaultOffset;
    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider2D>();
        _defaultOffset = _collider.offset;
    }
    
    public override Vector2 GetDestination(Vector3 ratPosition)
    {
        Vector2 direction = ratPosition - transform.position;

        var hit = Physics2D.Raycast(ratPosition, direction, Mathf.Infinity, ItemManager.Instance.VisualObstaclesMask);
        

        if (!hit || !hit.collider) return ratPosition;

        var destination = hit.point - direction.normalized * 0.3f; //hardcodeado lo se perdon es una jam 

        _collider.offset = destination - (Vector2) transform.position;
        
        return destination;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var rat = other.GetComponentInParent<RatController>();
        if (!rat) return;
        
        _collider.offset = _defaultOffset;
        
        rat.OnItemCollision(this);
    }

    public override void OnUnsetAsTarget()
    {
        base.OnUnsetAsTarget();
        _collider.offset = _defaultOffset;
    }
}
