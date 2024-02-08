using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class UITimer : MonoBehaviour
{
    [FormerlySerializedAs("text")] [SerializeField] private TMP_Text _text;
    public float timer=00;
    int minutes;
    int seconds;
    string niceTime;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        minutes = Mathf.FloorToInt(timer / 60F);
        seconds = Mathf.FloorToInt(timer - minutes * 60);
        niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        _text.text = niceTime;
    }
    
}
