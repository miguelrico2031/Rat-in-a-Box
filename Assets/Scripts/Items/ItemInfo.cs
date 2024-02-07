using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public GameObject Prefab { get => _prefab; }
    public GameObject Dummy { get => _dummy; }
    public bool HasSmell { get => _hasSmell; }
    // public float Range { get => _range; }
    public ItemInteraction Interaction { get => _interaction; }
    public string PlaceAudioName {get => _placeAudioName;}
    public string InteractAudioName {get => _interactAudioName;}

    [SerializeField] private GameObject _prefab;
    [SerializeField] private GameObject _dummy;
    [SerializeField] private bool _hasSmell;
    // [SerializeField] private float _range;
    [SerializeField] private ItemInteraction _interaction;
    [SerializeField] private string _placeAudioName;
    [SerializeField] private string _interactAudioName;


}

public enum ItemInteraction
{
    Trap,
    Consumable,
    Permanent,
    Repulsive
}
