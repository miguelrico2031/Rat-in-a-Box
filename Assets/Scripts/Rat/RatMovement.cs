using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RatMovement : MonoBehaviour
{
    
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Transform _target;
    
    private Seeker _seeker;
    private AIPath _path;


    private void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _path = GetComponent<AIPath>();
    }

    private void Start()
    {
        
        _seeker.StartPath(transform.position, _target.position, OnEndPath);
    }

    private void OnEndPath(Path p) => Debug.Log("acabau");
}
