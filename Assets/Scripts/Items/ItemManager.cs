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
    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneStart;
        
        _items = FindObjectsOfType<AItem>().ToList();
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
         _items = FindObjectsOfType<AItem>().ToList();
    }

    public void PlaceItem(ItemInfo itemInfo, Vector2 position)
    {
        var instance = Instantiate(itemInfo.Prefab, position, Quaternion.identity, transform);
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

    public bool RemoveItem(AItem item)
    {
        if (!_items.Remove(item)) return false;
        
        Destroy(item.gameObject);
        
        ItemsUpdated?.Invoke(_items);
        
        //ANTON: sonido borrar objeto
        
        return true;
    }

    public IReadOnlyList<AItem> GetItems() => _items;



}
