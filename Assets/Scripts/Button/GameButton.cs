using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField] private bool _isEnd;
    
    private RatController _rat;
    private Vector3 _direction;
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rat = other.GetComponentInParent<RatController>();
        if (!_rat) return;
        
        if(!_isEnd) StartCoroutine(NotifyRat());
        
        
    }

    private IEnumerator NotifyRat()
    {
        yield return new WaitForSeconds(0.1f);
        _rat.Button = transform;
    }

    private void Update()
    {
        if (!_rat) return;

        _direction = transform.position - _rat.transform.position;
        if (_direction.sqrMagnitude < 0.025f)
        {
            _rat.PressButton(transform.localScale.x > 0 ? 1 : -1);
            _rat = null;
            GetComponentInChildren<Animator>().SetBool("Press", true);
        }
    }
}
