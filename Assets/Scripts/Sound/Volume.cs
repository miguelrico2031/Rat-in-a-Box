using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public float volumeValue;
    public Slider slider;

    private void Start()
    {
        LoadVolume();
    }

    public void ChangeVolume(float newValue)
    {
        AudioListener.volume = newValue;
        PlayerPrefs.SetFloat("volumeValue", AudioListener.volume);
    }

    public void LoadVolume()
    {
        volumeValue = PlayerPrefs.GetFloat("volumeValue", 0.5f);
        slider.value = volumeValue;
    }
}
