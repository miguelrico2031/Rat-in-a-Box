using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    [Serializable]
    public class LevelItem
    {
        public ItemInfo Item;
        public int Uses;
    }

    public string Scene;
    public int LevelTime;
    public int DialogueIndex;
    public LevelItem[] AvailableItems;
    public Level NextLevel;

}
