using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public class Phrase
    {
        public Dialogues.Speaker Speaker;
        [TextArea]public string Text;
    }

    public Phrase[] Phrases;
}

