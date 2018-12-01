using UnityEngine;

public interface ICollectible
{
    CollectibleType CollectibleType { get; }
    void OnEat();
}

public class Collectible : MonoBehaviour, ICollectible
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
        manager.SpawnNewItem();
        Destroy(gameObject);
    }
}

public enum CollectibleType
{
    Mooshroom,
    Egg,
    Mandragora
}