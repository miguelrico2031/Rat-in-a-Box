using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class RatController : MonoBehaviour
{
    public AItem CurrentTarget { get => _currentTarget; set => ChangeTarget(value); }
    public Transform Button { get => _button; set => SetButton(value); }
    public IRatState CurrentState { get => _currentState; set => ChangeState(value); }
    public bool IsAlive { get; private set; }
    public event Action<AItem> TargetChange;
    public Vector2 CurrentDirection { get; set; }
    
    public AIPath AIPath { get; private set; }
    public Seeker Seeker { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    [SerializeField] private float _itemCheckTime;

    private AItem _currentTarget;
    private IRatState _currentState;
    private AItem _lastPermamentTargetCollided;
    private string _currentAnimation;
    private bool _currentAnimationBlocks;
    private string _nextAnimation;
    private Transform _button;
    private bool _pressingButton;
    private bool _wasAIStopped;

    private void Awake()
    {
        IsAlive = true;
        Seeker = GetComponent<Seeker>();
        AIPath = GetComponent<AIPath>();
        Animator = GetComponentInChildren<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();

        CurrentTarget = null;
        CurrentState = null;
    }

    private void Start()
    {
        ItemManager.Instance.ItemsUpdated += OnItemsUpdated;

        GameManager.Instance.GameStateChange += OnGameStateChange;
    }

    private void Update()
    {
        if(!_pressingButton) CurrentState?.Update();
    }

    public void OnGameStateChange(GameManager.GameState newState)
    {
        if (newState == GameManager.GameState.Playing)
        {
            TrySetTarget(ItemManager.Instance.GetItems());
        }
    }


    private void OnItemsUpdated(IReadOnlyList<AItem> items)
    {
        TrySetTarget(items);
    }

    private void ChangeState(IRatState newState)
    {
        _currentState?.Exit(newState);
        var last = _currentState;
        _currentState = newState;
        _currentState?.Enter(this, last);
    }

    private void ChangeTarget(AItem newTarget)
    {
        if(_currentTarget != null)  _currentTarget.OnUnsetAsTarget();
        _currentTarget = newTarget;
        if(_currentTarget != null) _currentTarget.OnSetAsTarget();
        
        TargetChange?.Invoke(_currentTarget);
    }

    public void TrySetTarget(IReadOnlyList<AItem> items, bool setState = true)
    {
        if (!IsAlive || Button != null) return;
        if (CurrentTarget && CurrentTarget.IsCovered) CurrentTarget = null;
        
        AItem target = null;
        AItem visualTarget = null;
        float bestDistance = Mathf.Infinity;
        foreach (var item in items)
        {
            if (!IsValidTarget(item, out var distance)) continue;

            if (!item.Info.HasSmell)
            {
                if (visualTarget == null || distance < bestDistance)
                {
                    visualTarget = item;
                    bestDistance = distance;
                }
            }

            if (visualTarget != null) continue;
            
            if (distance < bestDistance)
            {
                bestDistance = distance;
                target = item;
            }
        }

        if (visualTarget != null) target = visualTarget;

        if (CurrentTarget && !target) return;
        if (CurrentTarget && target == CurrentTarget) return;
        
        CurrentTarget = target && !target.IsCovered ? target : null;

        if (!setState) return;
        
        if (!CurrentTarget && CurrentState is not Idle)
        {
            CurrentState = new Idle();
        }
        else if (CurrentTarget)
        {
            
            if (_lastPermamentTargetCollided && _lastPermamentTargetCollided == CurrentTarget &&
                bestDistance < 0.6f)
            {
                CurrentTarget = null;
                CurrentState = new Idle();
            }
            else
            {
                CurrentState = new WalkToDestination();
            }
        }
    }

    private bool IsValidTarget(AItem item, out float distance)
    {
        distance = -1f;
        if (!item.IsInRange(transform)) return false;
        
        Vector2 target = item.transform.position;
        Vector2 origin = transform.position;
        Vector2 direction = target - origin;
        distance = direction.magnitude;

        if (item.Info.HasSmell) return true;

        
        var hit = Physics2D.Raycast(origin, direction, distance, ItemManager.Instance.VisualObstaclesMask);

        if (hit && hit.collider) return false;

        return true;
    }

    /*public IEnumerator PlayStepsSounds()
    {
        while (CurrentState is WalkToDestination)
        {
            //ANTON: elegir sonido random
            //MusicManager.Instance.PlaySound("");
            //yield return new WaitForSeconds(_stepSoundDelay);
        }
    }*/
    public IEnumerator CheckForItems()
    {
        while (CurrentState is WalkToDestination && Button == null)
        {
            TrySetTarget(ItemManager.Instance.GetItems());
            yield return new WaitForSeconds(_itemCheckTime);
        }
    }

    public void OnItemCollision(AItem item)
    {
        if (Button) return;
        //ANTON: aqui sonido de interactuar con objeto
        //item.Info.InteractAudioName
        switch (item.Info.InteractAudioName)
        {
            case "gato":
                MusicManager.Instance.PlaySound("muere");
                break;
            case "queso":
                MusicManager.Instance.PlaySound("comeQueso");
                break;

        }
        
        switch (item.Info.Interaction)
        {
            case ItemInteraction.Trap:
                if(item.Info.HasSmell) item.GetComponentInChildren<UnityEngine.Animator>().SetBool("Trap", true);
                CurrentState = new OnTrap(item.Info.HasSmell);
                break;
            
            case ItemInteraction.Consumable:
                if (CurrentTarget != item) break;
                CurrentTarget = null;
                ItemManager.Instance.RemoveItem(item);
                PlayOneTimeAnimationXY("Cheese", CurrentDirection);
                TrySetTarget(ItemManager.Instance.GetItems());
                if(CurrentState is Idle) PlayLoopingAnimationXY("Idle", CurrentDirection);
                break;
            
            case ItemInteraction.Permanent:
                if (CurrentTarget != item) break;
                CurrentTarget = null;
                _lastPermamentTargetCollided = item;
                PlayOneTimeAnimationXY("Heart", CurrentDirection);
                CurrentState = new Idle();
                break;
            
            case ItemInteraction.Repulsive:
                if (CurrentTarget != item) break;
                CurrentTarget = null;
                TrySetTarget(ItemManager.Instance.GetItems(), false);
                if (CurrentTarget == null) break;
                if (item == CurrentTarget) CurrentState = new Idle();
                else CurrentState = new WalkToDestination();
                break;
        }

    }

    public void PlayLoopingAnimationXY(string anim, Vector2 direction)
    {
        if (!IsAlive) return;
        if (_currentAnimationBlocks)
        {
            _nextAnimation = anim;
            return;
        }
        CurrentDirection = direction;
        Renderer.flipX = direction.x < 0f;

        if (direction.y < 0f && _currentAnimation != $"{anim} Front")
        {
            Animator.Play($"{anim} Front");
            _currentAnimation = $"{anim} Front";
        }

        if (direction.y >= 0f && _currentAnimation != $"{anim} Back")
        {
            Animator.Play($"{anim} Back");
            _currentAnimation = $"{anim} Back";

        }
    }


    public void PlayOneTimeAnimationXY(string anim, Vector2 direction)
    {
        if (_currentAnimationBlocks || !IsAlive) return;

        CurrentDirection = direction;
        _nextAnimation = null;
        _currentAnimationBlocks = true;
        _wasAIStopped = AIPath.isStopped;
        AIPath.isStopped = true;
        Renderer.flipX = direction.x < 0f;
        if (direction.y < 0f)
        {
            Animator.Play($"{anim} Front");
            _currentAnimation = $"{anim} Front";
        }
        else
        {
            Animator.Play($"{anim} Back");
            _currentAnimation = $"{anim} Back";
        }
        float length = Animator.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(HandleOneTimeAnimationXY(length, direction));
    }

    public void PlayOneTimeAnimationX(string anim, int x)
    {
        if (_currentAnimationBlocks) return;
        _nextAnimation = null;

        _currentAnimationBlocks = true;
        _wasAIStopped = AIPath.isStopped;
        AIPath.isStopped = true;
        Renderer.flipX = x < 0f;
        Animator.Play(anim);
        _currentAnimation = anim;
        
        float length = Animator.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(HandleOneTimeAnimationX(length, x));
    }

    private IEnumerator HandleOneTimeAnimationXY(float length, Vector2 direction)
    {
        yield return new WaitForSeconds(length);
        _currentAnimationBlocks = false;
        AIPath.isStopped = false;
        if (_nextAnimation == null) yield break;
        PlayLoopingAnimationXY(_nextAnimation, direction);
    }
    
    private IEnumerator HandleOneTimeAnimationX(float length, int x)
    {
        yield return new WaitForSeconds(length);
        _currentAnimationBlocks = false;
         AIPath.isStopped = _wasAIStopped;
    }

    private void SetButton(Transform button)
    {
        _button = button;

        CurrentState = new WalkToDestination();
    }

    public void PressButton(int direction)
    {
        PlayOneTimeAnimationX("Press Button", direction);
        CurrentState = null;
        StartCoroutine(Pinch());
    }

    private IEnumerator Pinch()
    {
        AIPath.isStopped = true;
        transform.Find("Arrow").gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        FindObjectOfType<CameraControls>().ZoomToRat(transform.position);
        var hand = GameObject.Find("Boss Hand");
        hand.transform.position = transform.position;
        hand.GetComponentInChildren<Animator>().SetBool("Pinch", true);
        yield return new WaitForSeconds(2f);
        //FADE OUT;
        SceneManager.LoadScene(GameManager.Instance.CurrentLevel.NextLevel.Scene);
    }
    
    
    

    public IEnumerator Die()
    {
        IsAlive = false;
        AIPath.isStopped = true;
        transform.Find("Arrow").gameObject.SetActive(false);
        GameManager.Instance.StopTimer();

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(GameManager.Instance.CurrentLevel.Scene);
    }

    private void OnDestroy()
    {
        ItemManager.Instance.ItemsUpdated -= OnItemsUpdated;

        GameManager.Instance.GameStateChange -= OnGameStateChange;
    }
}