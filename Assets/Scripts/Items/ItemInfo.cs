using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public GameObject Prefab { get => _prefab; }
    public bool HasSmell { get => _hasSmell; }
    // public float Range { get => _range; }
    public ItemInteraction Interaction { get => _interaction; }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _hasSmell;
    // [SerializeField] private float _range;
    [SerializeField] private ItemInteraction _interaction;


}

public enum ItemInteraction
{
    Trap,
    Consumable,
    Permanent,
    Repulsive
}
