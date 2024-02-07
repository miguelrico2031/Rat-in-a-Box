using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }
    
    public ItemInfo ItemToPlace { get; private set; }

    private GameObject _dummy;
    private Mouse _mouse;
    private Camera _cam;
    private Vector3 _to2D = new(1f, 1f, 0f);
    
    private void Awake()
    {
        if(Instance) Destroy(gameObject);
        Instance = this;
        SceneManager.sceneLoaded += OnSceneStart;
        
        _mouse = Mouse.current;
        _cam = Camera.main;
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        _mouse = Mouse.current;
        _cam = Camera.main;
    }

    public void SetItemToPlace(ItemInfo item)
    {
        ItemToPlace = item;
        if(_dummy) Destroy(_dummy);
        _dummy = Instantiate(ItemToPlace.Dummy);
    }

    private void Update()   
    {
        if (_dummy == null) return;
        
        Vector2 pos = _cam.ScreenToWorldPoint(_mouse.position.value);
        
        _dummy.transform.position = pos;
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.started || !ItemToPlace) return;

        var pos = _dummy.transform.position;
        
        if (Physics2D.OverlapCircle(pos, .2f)) return;

        Destroy(_dummy);
        ItemManager.Instance.PlaceItem(ItemToPlace, pos);
        _dummy = null;
        ItemToPlace = null;
    }

    public void CancelItemToPlace()
    {
        ItemToPlace = null;
        if (_dummy)
        {
            Destroy(_dummy);
            _dummy = null;
        }
    }
}
