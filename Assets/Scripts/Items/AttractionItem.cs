using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractionItem : AItem
{
    public override Vector2 GetTarget(Vector3 ratPosition) => transform.position;
}
