using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// [RequireComponent(typeof(IPathfinder))]
public class RatMovement : MonoBehaviour
{
    
    public TileBase CurrentTile { get; private set; }


    [SerializeField] private float _moveSpeed;
    
    private IPathfinder _pathfinder;
    private Rigidbody2D _rb;
    private TileBase _targetTile;
    private Vector2 _targetDirection;

    private void Awake()
    {
        _pathfinder = GetComponent<IPathfinder>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
       _rb.velocity = Time.fixedDeltaTime * _moveSpeed * new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));


    }

    // private void MoveToTile()
    // {
    //     _targetTile = _pathfinder.GetNextTile(CurrentTile);
    //
    //     if (!_targetTile) return;
    //     
    //     _targetDirection = _targetTile.
    // }
    
    
}
