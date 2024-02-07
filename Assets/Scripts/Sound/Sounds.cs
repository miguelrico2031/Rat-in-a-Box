using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/Sounds")]
public class Sounds : ScriptableObject
{
    public IReadOnlyList<Sound> SoundsList { get => _soundsList; }
    
    [SerializeField] private List<Sound> _soundsList;

}

[System.Serializable]
public class Sound
{
    [FormerlySerializedAs("name")] public string Name;
    [FormerlySerializedAs("clip")] public AudioClip Clip;
}
