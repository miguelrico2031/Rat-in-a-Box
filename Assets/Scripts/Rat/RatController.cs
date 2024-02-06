using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public AItem CurrentTarget { get; private set; }
    public IRatState CurrentState { get => _currentState; set => ChangeState(value); }
    
    public AIPath AIPath { get; private set; }
    public Seeker Seeker { get; private set; }
    public Animator Animator { get; private set; }
    public SpriteRenderer Renderer { get; private set; }

    [SerializeField] private float _itemCheckTime;
    [SerializeField] private LayerMask _visualObstacles; //obstaculos 
    
    private IRatState _currentState;

    private void Awake()
    {
        Seeker = GetComponent<Seeker>();
        AIPath = GetComponent<AIPath>();
        Animator = GetComponentInChildren<Animator>();
        Renderer = GetComponentInChildren<SpriteRenderer>();

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

    public void TrySetTarget(IReadOnlyList<AItem> items)
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
        
        if (!CurrentTarget && CurrentState is not Idle)
        {
            CurrentState = new Idle();
            
        }
        else if (CurrentTarget)
        {
            CurrentState = new WalkToTarget();
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

        
        var hit = Physics2D.Raycast(origin, direction, distance, _visualObstacles);

        if (hit && hit.collider) return false;

        return true;
    }


    public IEnumerator CheckForItems()
    {
        TrySetTarget(ItemManager.Instance.GetItems());
        yield return new WaitForSeconds(_itemCheckTime);
        if (CurrentState is WalkToTarget) yield return CheckForItems();
    }

    public void OnItemCollision(AItem item)
    {
        if (item != CurrentTarget)
        {
            return;
        }

        ItemManager.Instance.RemoveItem(item);

        CurrentTarget = null;
        TrySetTarget(ItemManager.Instance.GetItems());
    }
}
