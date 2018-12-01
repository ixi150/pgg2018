using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType CollectibleType { get { return _collectibleType; } }

    [SerializeField]
    private CollectibleType _collectibleType;
}

public enum CollectibleType
{
    Mooshroom,
    Egg,
    Mandragora
}