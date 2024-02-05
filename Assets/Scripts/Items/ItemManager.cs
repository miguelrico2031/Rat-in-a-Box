using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public event Action ItemsUpdated;
    
    private List<IItem> _items;
    private void Awake()
    {
         _items = new(GetComponentsInChildren<IItem>());
    }

    public void PlaceItem(ItemInfo itemInfo, Vector2 position)
    {
        var instance = Instantiate(itemInfo.Prefab, position, Quaternion.identity, transform);
        _items.Add(instance.GetComponent<IItem>());
        
    }

    public bool RemoveItem(IItem item)
    {
        if (_items.Remove(item))
        {
            
        }
        
    }
}
