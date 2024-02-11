using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    [SerializeField] private bool _isEnd;
    
    private RatController _rat;
    private Vector3 _direction;
    private Transform _ratD;
    private bool _ratOnArm;
    private Transform[] _waypointsArm;
    private int _wpI;
    
    private int randomIndex;
    private string soundName;
    private const float maxSonido= 10.0f; 
    private float pasoSonido = maxSonido;
    private float veloSonido = 50.0f;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _rat = other.GetComponentInParent<RatController>();
        if (!_rat) return;

        if (!_isEnd) StartCoroutine(NotifyRat());

        else {
            StartCoroutine(EndSequence(_rat));
        }
    }

    private IEnumerator NotifyRat()
    {
            // ANTON sonido de terminar
            MusicManager.Instance.PlaySound("endLevel");
            MusicManager.Instance.StopMusic(1.0f);

        yield return new WaitForSeconds(0.1f);
        _rat.Button = transform;
    }

    private void Update()
    {
        if (_rat)
        {
            _direction = transform.position - _rat.transform.position;
            if (_direction.sqrMagnitude < 0.025f)
            {
                _rat.PressButton(transform.localScale.x > 0 ? 1 : -1);
                _rat = null;
                GetComponentInChildren<Animator>().SetBool("Press", true);
            }
        }
        if (_ratD)
        {
            if (pasoSonido > maxSonido)
            {
                pasoSonido=0;
                randomIndex = UnityEngine.Random.Range(0, 3);
                soundName = "Anda" + randomIndex;
                MusicManager.Instance.PlaySound(soundName);
            }
            else
            {
                pasoSonido+=Time.deltaTime*veloSonido;
            }
            
            
            if (_ratOnArm)
            {
                _direction = _waypointsArm[_wpI].position - _ratD.position;
                if (_direction.sqrMagnitude < 0.025f)
                {
                    _wpI++;
                    if (_wpI == _waypointsArm.Length)
                    {
                        //FIN
                        _ratD = null;
                        Destroy(GameManager.Instance.gameObject);
                        SceneManager.LoadScene("CreditScene");
                    }

                }
                else
                {
                    _ratD.Translate(_direction.normalized * Time.deltaTime * 1.75f);
                }

                return;
            }
            _direction = transform.position - _ratD.position;
            if (_direction.sqrMagnitude < 0.025f)
            {
                _ratOnArm = true;
                _wpI = 0;
                _ratD.GetComponentInChildren<Animator>().SetTrigger("Move Back");
            }
            else _ratD.Translate(_direction.normalized * Time.deltaTime * 1.75f);
        }

    }

    private IEnumerator EndSequence(RatController rat)
    {
        rat.DisableAI();
        PlacementManager.Instance.CanPlace = false;
        
        Transform studentHand = GameObject.Find("Student Hand").transform;
        Animator studentAnim = studentHand.GetComponentInChildren<Animator>();

        GameObject props = GameObject.Find("End Level Props");
        var lid = props.transform.Find("Lid");
        var ratTrap = props.transform.Find("Rat Trap");
        var ratD = props.transform.Find("Rat");

        yield return new WaitForSeconds(1f);
        
        studentHand.position = rat.transform.position;
        studentAnim.Play("Place");
        yield return new WaitForSeconds(.5f);
        lid.position = rat.transform.position;
        lid.GetComponentInChildren<SpriteRenderer>().enabled = true;
        rat.gameObject.SetActive(false);
        MusicManager.Instance.PlaySound("metalPipe");
        yield return new WaitForSeconds(1.5f);
        
        studentHand.position = transform.position;
        studentAnim.Play("Place");
        yield return new WaitForSeconds(.5f);
        ratTrap.position = transform.position;
        ratTrap.GetComponentInChildren<SpriteRenderer>().enabled = true;
        MusicManager.Instance.PlaySound("poneRaton");

        yield return new WaitForSeconds(2.5f);
        
        FindObjectOfType<CameraControls>().ZoomToRat(transform.position);

        yield return new WaitForSeconds(0.5f);
        
        var hand = GameObject.Find("Boss Hand");
        hand.transform.position = transform.position;
        hand.GetComponentInChildren<Animator>().Play("Shock");
        yield return new WaitForSeconds(.5f);
        MusicManager.Instance.PlaySound("electrocutarLargo");
        GameManager.Instance.ElectricParticles(transform.position);

        yield return new WaitForSeconds(1.5f);

        ratD.transform.position = rat.transform.position;
        ratD.GetComponentInChildren<Animator>().SetTrigger("Move Front");
        var ratSprite = ratD.GetComponentInChildren<SpriteRenderer>();
        ratSprite.enabled = true;
        _ratD = ratD.transform;
        var handSp = hand.GetComponentInChildren<SpriteRenderer>();
            _waypointsArm = new Transform[]{ handSp.transform.GetChild(0), handSp.transform.GetChild(1), handSp.transform.GetChild(2) };
            
        

    }
}
