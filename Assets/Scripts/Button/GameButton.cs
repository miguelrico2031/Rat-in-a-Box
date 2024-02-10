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

        if (!_isEnd) StartCoroutine(NotifyRat());

        else StartCoroutine(EndSequence(_rat));
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

    private IEnumerator EndSequence(RatController rat)
    {
        FindObjectOfType<CameraControls>().ZoomToRat(rat.transform.position);
        rat.DisableAI();
        PlacementManager.Instance.CanPlace = false;
        
        Transform studentHand = GameObject.Find("Student Hand").transform;
        Animator studentAnim = studentHand.GetComponentInChildren<Animator>();

        GameObject props = GameObject.Find("End Level Props");
        var lid = props.transform.Find("Lid");
        var ratTrap = props.transform.Find("Rat Trap");

        yield return new WaitForSeconds(1f);
        
        studentHand.position = rat.transform.position;
        studentAnim.Play("Place");
        yield return new WaitForSeconds(.5f);
        lid.position = rat.transform.position;
        lid.GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(1f);
        
        studentHand.position = transform.position;
        studentAnim.Play("Place");
        yield return new WaitForSeconds(.5f);
        ratTrap.position = transform.position;
        ratTrap.GetComponentInChildren<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(2f);
        
        var hand = GameObject.Find("Boss Hand");
        hand.transform.position = transform.position;
        hand.GetComponentInChildren<Animator>().SetBool("Pinch", true);
        
        
    }
}
