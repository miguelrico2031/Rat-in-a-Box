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
    private bool _isFirstItem;
    private Transform _studentHand;
    private Animator _studentAnim;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneStart;
        
        _items = FindObjectsOfType<AItem>().ToList();
        _lids = new();
        _isFirstItem = true;
        _studentHand = GameObject.Find("Student Hand").transform;
        _studentAnim = _studentHand.GetComponentInChildren<Animator>();
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        _items = FindObjectsOfType<AItem>().ToList();
        _lids = new();
        _isFirstItem = true;

        _studentHand = GameObject.Find("Student Hand").transform;
        _studentAnim = _studentHand.GetComponentInChildren<Animator>();
    }

    public IEnumerator PlaceItem(ItemInfo itemInfo, Vector2 position)
    {
        _studentHand.position = position;
        _studentAnim.Play("Place");
        yield return new WaitForSeconds(.5f);
        
        var instance = Instantiate(itemInfo.Prefab, position, Quaternion.identity);
        _items.Add(instance.GetComponent<AItem>());

        if (_isFirstItem)
        {
            _isFirstItem = false;
            GameManager.Instance.State = GameManager.GameState.Playing;
        }
        GameManager.Instance.Use(itemInfo);
        HUD.Instance.UpdateUses(itemInfo);
        ItemsUpdated?.Invoke(_items);
        
        //ANTON: sonido poner objeto
        //itemInfo.PlaceAudioName
        switch (itemInfo.PlaceAudioName) // XDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
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

    public IEnumerator PlaceLid(ItemInfo lidInfo, AItem item)
    {
        _studentHand.position = item.transform.position;
        _studentAnim.Play("Place");
        yield return new WaitForSeconds(.5f);
        
        _lids.Add(Instantiate(lidInfo.Prefab, item.transform.position, Quaternion.identity), item);
        
        item.ToggleLid(true);
        SelectionManager.Instance.Selected = null;
        GameManager.Instance.Use(lidInfo);
        HUD.Instance.UpdateUses(lidInfo);
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
    
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneStart;
    }
    
}
