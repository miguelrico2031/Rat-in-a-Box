using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public GameObject Prefab { get => _prefab; }

    [SerializeField] private GameObject _prefab;
}
