using UnityEngine;
using FMODUnity;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class FMODEvent
    {
        public string eventName;
        public EventReference eventRef;
        [HideInInspector]
        public FMOD.Studio.EventInstance eventInstance;
    }

    public List<FMODEvent> events;

    void Start()
    {
        foreach (var fmodEvent in events)
        {
            // Crea instancias de todos los eventos
            fmodEvent.eventInstance = RuntimeManager.CreateInstance(fmodEvent.eventRef);
        }
    }

    public void PlayEvent(string eventName)
    {
        var fmodEvent = events.Find(e => e.eventName == eventName);
        
        fmodEvent.eventInstance.setParameterByName("escape", 1); // SETEO MANUAL
        
        if (fmodEvent != null)
        {
            fmodEvent.eventInstance.start();
            Debug.Log("Reproduciendo evento: " + eventName);
        }
        else
        {
            Debug.LogWarning("Event not found: " + eventName);
        }
    }

    public void StopEvent(string eventName)
    {
        var fmodEvent = events.Find(e => e.eventName == eventName);
        if (fmodEvent != null)
        {
            fmodEvent.eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }

    public void SetEventParameter(string eventName, string parameterName, int value)
    {
        var fmodEvent = events.Find(e => e.eventName == eventName);
        if (fmodEvent != null)
        {
            fmodEvent.eventInstance.setParameterByName(parameterName, value);
            Debug.Log("Parametro: " + parameterName + " = " + value);
        }
    }

    void OnDestroy()
    {
        foreach (var fmodEvent in events)
        {
            fmodEvent.eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            fmodEvent.eventInstance.release();
        }
    }
}
