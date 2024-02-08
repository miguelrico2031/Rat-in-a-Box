using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance { get; private set; }
    
    [SerializeField] private Dialogues _dialogues;
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Image _speakerImage;
    [SerializeField] private TextMeshProUGUI _speakerNameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private float _typeSpeed;

    private Dialogue _currentDialogue;
    private int _phraseIndex;
    private bool _phraseFinished, _skip;

    private Action _callback;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += OnSceneStart;
        
        _dialogueUI.SetActive(false);
        
    }

    private void OnSceneStart(Scene s, LoadSceneMode m)
    {
        //_dialogueUI.SetActive(false);
    }

    public void StartDialogue(int index, Action callback)
    {
        _currentDialogue = _dialogues.GetDialogue(index);
        _phraseIndex = -1;
        _skip = false;
        _dialogueUI.SetActive(true);
        _callback = callback;
        DisplayNextPhrase();
    }

    private void DisplayNextPhrase()
    {
        _phraseIndex++;

        if (_phraseIndex >= _currentDialogue.Phrases.Length)
        {
            _dialogueUI.SetActive(false);
            _callback.Invoke();
            return;
        }
        
        var phrase = _currentDialogue.Phrases[_phraseIndex];
        var data = _dialogues.GetSpeakerData(phrase.Speaker);
        _speakerImage.sprite = data.Sprite;
        _speakerNameText.text = data.Name;
        StartCoroutine(TypePhrase(phrase.Text));
    }

    private IEnumerator TypePhrase(string text)
    {
        _phraseFinished = false;
        _dialogueText.text = "";
        float delay = 1f / _typeSpeed;  
        foreach (char c in text)
        {
            //ANTON SONIDO DIALOGO
            _dialogueText.text += c;
            if(!_skip) yield return new WaitForSeconds(delay);
        }

        _skip = false;
        _phraseFinished = true;
    }

    public void NextButtonPress()
    {
        if (!_phraseFinished) _skip = true;
        else DisplayNextPhrase();
        
    }
}
