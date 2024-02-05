using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using Rat;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RatController : MonoBehaviour
{
    public AItem CurrentTarget { get; private set; }
    public IRatState CurrentState { get => _currentState; set => ChangeState(value); }
    
    public AIPath AIPath { get; private set; }
    public Seeker Seeker { get; private set; }

    [SerializeField] private LayerMask _visualObstacles; //obstaculos 
    
    private IRatState _currentState;

    private void Awake()
    {
        Seeker = GetComponent<Seeker>();
        AIPath = GetComponent<AIPath>();

        CurrentState = null;
    }

    private void Start()
    {
        ItemManager.Instance.ItemsUpdated += OnItemsUpdated;
        
        StartAI();
    }

    private void Update()
    {

    }

    public void StartAI()
    {
        if (CurrentState != null) return;
        OnItemsUpdated(ItemManager.Instance.GetItems());
    }


    private void OnItemsUpdated(IReadOnlyList<AItem> items)
    {
        CurrentTarget = null;
        float bestDistance = Mathf.Infinity;
        foreach (var item in items)
        {
            if (!IsValidTarget(item, out var distance)) continue;
            

            if (distance < bestDistance)
            {
                bestDistance = distance;
                CurrentTarget = item;
            }
        }

        if (!CurrentTarget)
        {
            CurrentState = new Idle();
            
        }
        else
        {
            CurrentState = new WalkToTarget();
        }
    }

    private void ChangeState(IRatState newState)
    {
        _currentState?.Exit(newState);
        var last = _currentState;
        _currentState = newState;
        _currentState?.Enter(this, last);
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
}
