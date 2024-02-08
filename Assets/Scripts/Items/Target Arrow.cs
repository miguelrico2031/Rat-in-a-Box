using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TargetArrow : MonoBehaviour
{
    [SerializeField] private float _lerpSpeed;
    
    private Transform _rat, _target;
    private SpriteRenderer _spriteRenderer;

    private Behaviour _behaviour;
    private enum Behaviour
    {
        GoToTarget, GoToPlayer, None, StayOnTarget
    }
    
    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _behaviour = Behaviour.None;
    }

    private void Start()
    {
        var rat =FindObjectOfType<RatController>();
        _rat = rat.transform;
        rat.TargetChange += OnTargetChange;
    }

    private void Update()
    {
        switch (_behaviour)
        {
            case Behaviour.GoToPlayer:
                transform.position = Vector3.Lerp(transform.position, _rat.position, Time.deltaTime * _lerpSpeed);
                if ((transform.position - _rat.position).sqrMagnitude < .01f)
                {
                    _spriteRenderer.enabled = false;
                    _behaviour = Behaviour.None;
                }
                break;
            
            case Behaviour.GoToTarget:
                transform.position = Vector3.Lerp(transform.position, _target.position, Time.deltaTime * _lerpSpeed);
                if ((transform.position - _target.position).sqrMagnitude < .01f)
                {
                    _behaviour = Behaviour.StayOnTarget;
                }
                break;
        }
        

    }

    private void OnTargetChange(AItem newTarget)
    {
        if (newTarget == null)
        {
            _target = null;
            
            if (_behaviour != Behaviour.None) _behaviour = Behaviour.GoToPlayer;
            
            return;
        }


        if (_behaviour == Behaviour.None)
        {
            _spriteRenderer.enabled = true;
            transform.position = _rat.position;
        }

        _behaviour = Behaviour.GoToTarget;
        _target = newTarget.transform;
        _spriteRenderer.enabled = true;
    }
}
