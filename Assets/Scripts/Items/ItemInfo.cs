using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public GameObject Prefab { get => _prefab; }
    public bool HasSmell { get => _hasSmell; }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _hasSmell;
}
