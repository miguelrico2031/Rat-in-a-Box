using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class RatController : MonoBehaviour
{
    public Transform CurrentTarget { get; private set; }
    public IRatState CurrentState { get; private set; }
    
    public AIPath AIPath { get; private set; }
    public Seeker Seeker { get; private set; }

    private void Awake()
    {
        Seeker = GetComponent<Seeker>();
        AIPath = GetComponent<AIPath>();
    }
}
