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
        float newVol = AudioListener.volume;
        newVol = newValue;
        AudioListener.volume = newVol;
        PlayerPrefs.SetFloat("volumeValue", AudioListener.volume);
        PlayerPrefs.SetFloat("slider", newValue);
    }

    public void LoadVolume()
    {
        volumeValue = PlayerPrefs.GetFloat("volumeValue");
        slider.value = PlayerPrefs.GetFloat("slider");
    }
}
