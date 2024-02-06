using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    
    public event Action<IReadOnlyList<AItem>> ItemsUpdated;
    public LayerMask ItemsMask { get => _itemsMask; }
    public LayerMask VisualObstaclesMask { get => _visualObstaclesMask; }
    
    [SerializeField] private LayerMask _itemsMask; 
    [SerializeField] private LayerMask _visualObstaclesMask; //obstaculos 


    private List<AItem> _items;
    private void Awake()
    {
        // if(Instance) Destroy(gameObject);
        Instance = this;
        
         _items = GetComponentsInChildren<AItem>().ToList();
    }

    public void PlaceItem(ItemInfo itemInfo, Vector2 position)
    {
        var instance = Instantiate(itemInfo.Prefab, position, Quaternion.identity, transform);
        _items.Add(instance.GetComponent<AItem>());
        
        ItemsUpdated?.Invoke(_items);
        
    }

    public bool RemoveItem(AItem item)
    {
        if (!_items.Remove(item)) return false;
        
        Destroy(item.gameObject);
        
        ItemsUpdated?.Invoke(_items);
        return true;
    }

    public IReadOnlyList<AItem> GetItems() => _items;



}
