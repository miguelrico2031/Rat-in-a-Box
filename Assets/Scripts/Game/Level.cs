using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public class LevelItem
    {
        public ItemInfo Item;
        public int Uses;
    }
    
    public LevelItem[] _availableItems;
    public float LevelTime;
    
}
