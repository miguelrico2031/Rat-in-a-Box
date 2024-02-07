using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Serialization;

public class RatController : MonoBehaviour
{
    public AItem CurrentTarget { get => _currentTarget; set => ChangeTarget(value); }
    public IRatState CurrentState { get => _currentState; set => ChangeState(value); }
    
    public AIPath AIPath { get; private set; }
    public Seeker Seeker { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    [SerializeField] private float _itemCheckTime;

    private AItem _currentTarget;
    private IRatState _currentState;
    private AItem _lastPermamentTargetCollided;

    private void Awake()
    {
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
        
        StartAI();
    }

    private void Update()
    {
        CurrentState?.Update();
    }

    public void StartAI()
    {
        if (CurrentState != null) return;
        TrySetTarget(ItemManager.Instance.GetItems());
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
    }

    public void TrySetTarget(IReadOnlyList<AItem> items, bool setState = true)
    {
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
        
        CurrentTarget = target;

        if (!setState) return;
        
        if (!CurrentTarget && CurrentState is not Idle)
        {
            CurrentState = new Idle();
        }
        else if (CurrentTarget)
        {
            if (_lastPermamentTargetCollided && _lastPermamentTargetCollided == CurrentTarget)
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
        while (CurrentState is WalkToDestination)
        {
            TrySetTarget(ItemManager.Instance.GetItems());
            yield return new WaitForSeconds(_itemCheckTime);
        }
    }

    public void OnItemCollision(AItem item)
    {
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
                Die();
                break;
            
            case ItemInteraction.Consumable:
                if (CurrentTarget != item) break;
                CurrentTarget = null;
                ItemManager.Instance.RemoveItem(item);
                TrySetTarget(ItemManager.Instance.GetItems());
                break;
            
            case ItemInteraction.Permanent:
                if (CurrentTarget != item) break;
                CurrentTarget = null;
                _lastPermamentTargetCollided = item;
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

    private void Die()
    {
        Debug.Log("muelto");
        Destroy(gameObject);
    }
}