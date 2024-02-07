using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;
    private Dictionary<string, AudioClip> soundDictionary;
    
    private AudioSource musicSource;
    private AudioSource ambienceSource;
    private Coroutine fadeOutCoroutine;
    private Coroutine fadeOutAmbienceCoroutine;

    void Awake()
    {
        soundDictionary = new Dictionary<string, AudioClip>();
        musicSource = gameObject.AddComponent<AudioSource>();
        ambienceSource = gameObject.AddComponent<AudioSource>();
        
        foreach (var sound in sounds)
        {
            soundDictionary[sound.name] = sound.clip;
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
            Destroy(soundGameObject, clip.length);
        }
        else
        {
            Debug.LogWarning("Sound name not found: " + soundName);
        }
    }



/////////////////////////////////////////////////////////////////////////////////////////////////
    public void PlayMusic(string musicName, bool loop)
    {
        if (soundDictionary.TryGetValue(musicName, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music name not found: " + musicName);
        }
    }

    public void StopMusic(float fadeOutDuration)
    {
        if (fadeOutCoroutine != null)
        {
            StopCoroutine(fadeOutCoroutine);
        }
        fadeOutCoroutine = StartCoroutine(FadeOutMusic(fadeOutDuration));
    }

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVolume = musicSource.volume;

        while (musicSource.volume > 0)
        {
            musicSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;
    }


/////////////////////////////////////////////////////////////////////////////////////////////////
    public void PlayAmbience(string ambienceName, bool loop)
    {
        if (soundDictionary.TryGetValue(ambienceName, out AudioClip clip))
        {
            ambienceSource.clip = clip;
            ambienceSource.loop = loop;
            ambienceSource.Play();
        }
    }

    public void StopAmbience()
    {
        ambienceSource.Stop();
    }

    public void FadeOutAmbience(float fadeOutDuration)
    {
        if (fadeOutAmbienceCoroutine != null)
        {
            StopCoroutine(fadeOutAmbienceCoroutine);
        }
        fadeOutAmbienceCoroutine = StartCoroutine(FadeOut(ambienceSource, fadeOutDuration));
    }

    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
