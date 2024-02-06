using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionItem : AItem
{
    public override Vector2 GetDestination(Vector3 ratPosition) => transform.position;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var rat = other.GetComponentInParent<RatController>();
        if (!rat) return;
        
        rat.OnItemCollision(this);
    }

}
