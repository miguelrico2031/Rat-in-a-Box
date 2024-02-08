using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    
    public event Action<IReadOnlyList<AItem>> ItemsUpdated;
    public LayerMask ItemsMask { get => _itemsMask; }
    public LayerMask VisualObstaclesMask { get => _visualObstaclesMask; }
    
    [SerializeField] private LayerMask _itemsMask; 
    [SerializeField] private LayerMask _visualObstaclesMask; //obstaculos 


    private List<AItem> _items;
    private Dictionary<GameObject, AItem> _lids;
    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneStart;
        
        _items = FindObjectsOfType<AItem>().ToList();
        _lids = new();
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
         _items = FindObjectsOfType<AItem>().ToList();
         _lids = new();
    }

    public void PlaceItem(ItemInfo itemInfo, Vector2 position)
    {
        var instance = Instantiate(itemInfo.Prefab, position, Quaternion.identity);
        _items.Add(instance.GetComponent<AItem>());
        
        ItemsUpdated?.Invoke(_items);
        
        //ANTON: sonido poner objeto
        //itemInfo.PlaceAudioName
        switch (itemInfo.PlaceAudioName)
        {
            case "gato":
                MusicManager.Instance.PlaySound("poneGato");
                break;
            case "queso":
                MusicManager.Instance.PlaySound("poneQueso");
                break;
            case "peluche":
                MusicManager.Instance.PlaySound("poneRaton");
                break;
            case "spray":
                MusicManager.Instance.PlaySound("poneSpray");
                break;
        }
        
    }

    public void PlaceLid(ItemInfo lidInfo, AItem item)
    {
        _lids.Add(Instantiate(lidInfo.Prefab, item.transform.position, Quaternion.identity), item);
        
        item.ToggleLid(true);
        SelectionManager.Instance.Selected = null;
        
        ItemsUpdated?.Invoke((_items));
    }

    public bool RemoveItem(AItem item)
    {
        if (!_items.Remove(item)) return false;
        
        Destroy(item.gameObject);
        
        ItemsUpdated?.Invoke(_items);
        
        //ANTON: sonido borrar objeto
        MusicManager.Instance.PlaySound("borrar");
        
        return true;
    }
    
    public bool RemoveLid(GameObject lid)
    {
        if (!_lids.ContainsKey(lid)) return false;

        var item = _lids[lid];
        _lids.Remove(lid);
        Destroy(lid);
        item.ToggleLid(false);
        
        ItemsUpdated?.Invoke((_items));

        return true;
    }

    public IReadOnlyList<AItem> GetItems() => _items;
    
}
