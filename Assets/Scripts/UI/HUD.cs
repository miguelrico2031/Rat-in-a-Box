using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD Instance { get; private set; }

    [SerializeField] private GameObject _controlUI;
    [SerializeField] private GameObject _itemsUI;
    [SerializeField] private GameObject _timerUI;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private GameObject _cheeseButton;
    [SerializeField] private GameObject _poisonButton;
    [SerializeField] private GameObject _ratPlushButton;
    [SerializeField] private GameObject _catPlushButton;
    [SerializeField] private GameObject _lidButton;
    [SerializeField] private GameObject _removeButton;
    [SerializeField] private GameObject _cancelButton;
    [SerializeField] private TextMeshProUGUI _cheeseUses, _poisonUses, _ratPlushUses, _catPlushUses, _lidUses;
    [SerializeField] private Image _fade;

    [Header("Item Infos")] [SerializeField]
    private ItemInfo _cheese;
    [SerializeField] private ItemInfo _poison;
    [SerializeField] private ItemInfo _ratPlush;
    [SerializeField] private ItemInfo _catPlush;
    [SerializeField] private ItemInfo _lid;

    [Header("Pause Menu")]
    [SerializeField] private PauseMenuUI pauseMenuUI;


    private Dictionary<ItemInfo, GameObject> _itemButtons;
    private Dictionary<ItemInfo, TextMeshProUGUI> _itemUses;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneStart;

        _itemButtons = new()
        {
            {_cheese, _cheeseButton},
            {_poison, _poisonButton},
            {_ratPlush, _ratPlushButton},
            {_catPlush, _catPlushButton},
            {_lid, _lidButton}
        };
        
        _itemUses = new()
        {
            {_cheese, _cheeseUses},
            {_poison, _poisonUses},
            {_ratPlush, _ratPlushUses},
            {_catPlush, _catPlushUses},
            {_lid, _lidUses}
        };
        
        _controlUI.SetActive(false);
        _itemsUI.SetActive(false);
        _timerUI.SetActive(false);
    }

    private void Start()
    {
        SelectionManager.Instance.SelectedChange += OnSelectedChange;
        PlacementManager.Instance.ItemChange += OnItemChange;
        _removeButton.SetActive(false);
        _cancelButton.SetActive(false);
        GameManager.Instance.GameStateChange += OnGameStateChange;
        GameManager.Instance.TimerDecreased += UpdateTime;
        
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        _removeButton.SetActive(false);
        _cancelButton.SetActive(false);

        _fade.color = new(0f, 0f, 0f, 0f);
    }
    
    

    private void OnGameStateChange(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.Dialogue:
                _controlUI.SetActive(false);
                _itemsUI.SetActive(false);
                _timerUI.SetActive(false);
                break;
            case GameManager.GameState.Overview:
                _controlUI.SetActive(true);
                _itemsUI.SetActive(true);
                _timerUI.SetActive(true);
                UpdateTime(GameManager.Instance.CurrentLevel.LevelTime);
                foreach (var b in _itemButtons.Values)
                {
                    b.SetActive(false);
                }
                foreach (var li in GameManager.Instance.CurrentLevel.AvailableItems)
                {
                    _itemButtons[li.Item].SetActive(true);
                    int uses = GameManager.Instance.GetUses(li.Item);
                    if(uses <= 0) _itemUses[li.Item].transform.parent.gameObject.SetActive(false);
                    else
                    {
                        _itemUses[li.Item].transform.parent.gameObject.SetActive(true);
                        _itemUses[li.Item].text = $"{uses}";
                    }
                }
                
                break;
        }
    }


    private void UpdateTime(int time)
    {
        var minutes = time / 60;
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, time - minutes * 60);
    }
    private void OnSelectedChange()
    {
        _removeButton.SetActive(SelectionManager.Instance.Selected != null);
    }

    private void OnItemChange()
    {
        _cancelButton.SetActive(PlacementManager.Instance.ItemToPlace != null);
    }
    
    public void UpdateUses(ItemInfo item)
    {
        if (_itemUses[item] == null) return;
        
        _itemUses[item].text = $"{GameManager.Instance.GetUses(item)}";
    }

    public void SelectItem(ItemInfo item)
    {
        if(GameManager.Instance.CanUse(item)) PlacementManager.Instance.SetItemToPlace(item);
    }

    public void CancelItem()
    {
        PlacementManager.Instance.CancelItemToPlace();
    }

    public void RemoveItem()
    {
        var selectable = SelectionManager.Instance.Selected;
        if (!selectable) return;

        var nonLidItem = selectable.GetComponentInParent<AItem>();

        if (nonLidItem) ItemManager.Instance.RemoveItem(nonLidItem);
        else ItemManager.Instance.RemoveLid(selectable.transform.parent.gameObject);
    }

    public void Play()
    {
        GameManager.Instance.State = GameManager.GameState.Playing;
    }

    public void Pause()
    {

        pauseMenuUI.PauseGame();
    }

    public void Restart()
    {
        SceneManager.LoadScene(GameManager.Instance.CurrentLevel.Scene);
    }

    public void Fade(bool fadeIn, float duration, Action callback = null) => StartCoroutine(fadeIn ? FadeIn(duration, callback) : FadeOut(duration, callback));

    private IEnumerator FadeIn(float duration, Action callback)
    {
        Color c = new(0f, 0f, 0f, 1f);
        _fade.color = c;

        for (int i = 0; i < 20; i++)
        {
            c.a -= 0.05f;
            _fade.color = c;
            yield return new WaitForSeconds(0.05f * duration);
        }
        callback?.Invoke();
    }

    private IEnumerator FadeOut(float duration, Action callback)
    {
        Color c = new(0f, 0f, 0f, 0f);
        _fade.color = c;

        for (int i = 0; i < 20; i++)
        {
            c.a += 0.05f;
            _fade.color = c;
            yield return new WaitForSeconds(0.05f * duration);
        }
        callback?.Invoke();
    }
    
    
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneStart;
    }

}
