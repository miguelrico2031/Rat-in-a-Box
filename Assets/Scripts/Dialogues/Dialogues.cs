using System;
using UnityEngine;

    [CreateAssetMenu(menuName = "ScriptableObjects/Dialogues")]
    public class Dialogues : ScriptableObject
    {
        public enum Speaker
        {
            Boss, Student, Rat, UnnamedRat, UnnamedBoss
        }

        [Serializable]
        public class SpeakerData
        {
            public Speaker Speaker;
            public string Name;
            public Sprite Sprite;
        }

        [SerializeField] private SpeakerData[] _speakers;
        [SerializeField] private Dialogue[] _dialogues;

        public Dialogue GetDialogue(int idx) => _dialogues[idx];
        
        public SpeakerData GetSpeakerData(Speaker speaker)
        {
            foreach (var sd in _speakers) if (sd.Speaker == speaker) return sd;
            
            return null;
        }
    }
