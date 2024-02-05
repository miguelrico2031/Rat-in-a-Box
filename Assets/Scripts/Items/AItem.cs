
using UnityEngine;

public abstract class AItem : MonoBehaviour
{
    public ItemInfo Info { get => _info; }

    [SerializeField] private ItemInfo _info;
    
    
    public virtual bool IsInRange(Transform t)
    {
        return true;
    }
}
