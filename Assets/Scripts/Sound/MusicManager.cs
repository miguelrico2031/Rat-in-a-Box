using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private int _numberOfAudioSources;
    

    private Dictionary<string, AudioClip> _soundDictionary;

    [SerializeField] private Sounds _sounds;
    
    private AudioSource _musicSource;
    private AudioSource _ambienceSource;
    private Coroutine _fadeOutCoroutine;
    private Coroutine _fadeOutAmbienceCoroutine;

    private List<AudioSource> _audioSources;
    private GameObject _audioSourcesGO;

    void Awake()
    {

        if(Instance) Destroy(gameObject);
        Instance = this;
        // SceneManager.sceneLoaded += OnSceneStart;
        
        
        _soundDictionary = new Dictionary<string, AudioClip>();
        _musicSource = gameObject.AddComponent<AudioSource>();
        _ambienceSource = gameObject.AddComponent<AudioSource>();
        
        foreach (var sound in _sounds.SoundsList)
        {
            _soundDictionary[sound.Name] = sound.Clip;
        }

        _audioSources = new();
        _audioSourcesGO = new GameObject("Sound");

        for (int i = 0; i < _numberOfAudioSources; i++)
        {
             var src = _audioSourcesGO.AddComponent<AudioSource>();
             src.playOnAwake = false;
             src.enabled = false;
             _audioSources.Add(src);
        }
    }

    private AudioSource GetAudioSource()
    {
        foreach (var src in _audioSources)
        {
            if(src.enabled) continue;

            src.enabled = true;
            return src;
        }
        
        var newSrc = _audioSourcesGO.AddComponent<AudioSource>();
        newSrc.playOnAwake = false;
        _audioSources.Add(newSrc);
        return newSrc;
    }

    private void ReturnAudioSource(AudioSource src)
    {
        src.enabled = false;
    }
    
    // private void OnSceneStart(Scene s, LoadSceneMode m)
    // {
    //     
    // }

    public void PlaySound(string soundName)
    {
        if (_soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            StartCoroutine(PlayAndDisable(clip));
            return;
        }
        if(soundName != "") Debug.LogWarning("Sound name not found: " + soundName);
    }

    private IEnumerator PlayAndDisable(AudioClip clip, bool unscaledTime = false)
    {
        var src = GetAudioSource();
        src.clip = clip;
        src.Play();
        if (!unscaledTime) yield return new WaitForSeconds(clip.length);
        else yield return new WaitForSecondsRealtime(clip.length);

        ReturnAudioSource(src);
    }



/////////////////////////////////////////////////////////////////////////////////////////////////
    public void PlayMusic(string musicName, bool loop)
    {
        if (_soundDictionary.TryGetValue(musicName, out AudioClip clip))
        {
            _musicSource.clip = clip;
            _musicSource.loop = loop;
            _musicSource.Play();
            return;
        }
        Debug.LogWarning("Music name not found: " + musicName);
    }

    public void StopMusic(float fadeOutDuration)
    {
        if (_fadeOutCoroutine != null)
        {
            StopCoroutine(_fadeOutCoroutine);
            _fadeOutCoroutine = null;
        }
        _fadeOutCoroutine = StartCoroutine(FadeOut(_musicSource, fadeOutDuration));
    }

    private IEnumerator FadeOut(AudioSource src, float duration, bool unscaledTime = false)
    {
        float startVolume = src.volume;

        while (src.volume > 0)
        {
            if(!unscaledTime) src.volume -= startVolume * Time.deltaTime / duration;
            else src.volume -= startVolume * Time.unscaledDeltaTime / duration;
            yield return null;
        }

        _musicSource.Stop();
        _musicSource.volume = startVolume;
    }


/////////////////////////////////////////////////////////////////////////////////////////////////
    public void PlayAmbience(string ambienceName, bool loop)
    {
        if (!_soundDictionary.TryGetValue(ambienceName, out AudioClip clip)) return;
        
        _ambienceSource.clip = clip;
        _ambienceSource.loop = loop;
        _ambienceSource.Play();
        
    }

    public void StopAmbience()
    {
        _ambienceSource.Stop();
    }

    public void FadeOutAmbience(float fadeOutDuration)
    {
        if (_fadeOutAmbienceCoroutine != null)
        {
            StopCoroutine(_fadeOutAmbienceCoroutine);
            _fadeOutAmbienceCoroutine = null;
        }
        _fadeOutAmbienceCoroutine = StartCoroutine(FadeOut(_ambienceSource, fadeOutDuration));
    }


}
