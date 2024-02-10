using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }

    public event Action ItemChange;
    public ItemInfo ItemToPlace
    {
        get => _itemToPlace;
        private set
        {
            _itemToPlace = value;
            ItemChange?.Invoke();
        }
    }

    [SerializeField] private LayerMask _validLayersToPlace;
    
    private ItemInfo _itemToPlace;
    private GameObject _dummy;
    private Mouse _mouse;
    private Camera _cam;
    private Vector3 _to2D = new(1f, 1f, 0f);
    private Collider2D[] _overlaps;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneStart;

        _overlaps = new Collider2D[10];
            
        _mouse = Mouse.current;
        _cam = Camera.main;
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        _itemToPlace = null;
        if(_dummy) Destroy(_dummy);
        _dummy = null;
        _mouse = Mouse.current;
        _cam = Camera.main;
    }

    public void SetItemToPlace(ItemInfo item)
    {
        ItemToPlace = item;
        if(_dummy) Destroy(_dummy);
        _dummy = Instantiate(ItemToPlace.Dummy);
        SelectionManager.Instance.Selected = null;
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
        StartCoroutine(TryPlaceItem(_dummy.transform.position));
    }

    private IEnumerator TryPlaceItem(Vector3 pos)
    {
        yield return null;

        if (!ItemToPlace) yield break;
        
        if (!ItemToPlace.IsLid)
        {
            if (EventSystem.current.IsPointerOverGameObject()) yield break;
            var a = Physics2D.OverlapCircle(pos, .2f, ~( _validLayersToPlace));
            if(a) yield break;
            
            int n =  Physics2D.OverlapCircleNonAlloc(pos, .5f, _overlaps, ~(_validLayersToPlace));
            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (_overlaps[i].GetComponentInParent<RatController>()) yield break;
                }
            }
            
            Destroy(_dummy);
            StartCoroutine(ItemManager.Instance.PlaceItem(ItemToPlace, pos));
            _dummy = null;
            ItemToPlace = null;
            yield break;
        }
        
        int colsNumber = Physics2D.OverlapCircleNonAlloc(pos, .4f, _overlaps, ~(_validLayersToPlace));
        if (colsNumber == 0) yield break;

        float distance = Mathf.Infinity;
        AItem item = null;

        for (int i = 0; i < colsNumber; i++)
        {
            var it = _overlaps[i].GetComponentInParent<AItem>();
            if (!it)continue;
            float d = (transform.position - pos).sqrMagnitude;
            if (d < distance)
            {
                distance = d;
                item = it;
            }
        }
        
        if (!item) yield break;

        Destroy(_dummy);
        StartCoroutine(ItemManager.Instance.PlaceLid(ItemToPlace, item));
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
