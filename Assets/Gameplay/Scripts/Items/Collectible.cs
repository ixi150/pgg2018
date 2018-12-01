using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType CollectibleType { get { return _collectibleType; } }

    [SerializeField]
    private CollectibleType _collectibleType;

    private CollectibleManager manager;

    public void init(CollectibleManager manager)
    {
        this.manager = manager;
    }

    public void OnEat()
    {
        Destroy(gameObject);
    }
}

public enum CollectibleType
{
    Mooshroom,
    Egg,
    Mandragora
}