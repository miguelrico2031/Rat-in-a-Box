using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public GameObject Prefab { get => _prefab; }
    public bool HasSmell { get => _hasSmell; }
    public bool PlayerPlaced { get => _playerPlaced; }
    public float Range { get => _range; }

    [SerializeField] private GameObject _prefab;
    [SerializeField] private bool _hasSmell;
    [SerializeField] private bool _playerPlaced;
    [SerializeField] private float _range;
}
