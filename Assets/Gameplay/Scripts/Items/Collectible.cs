using System;
using UnityEngine;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

public interface ICollectible
{
    CollectibleType Type { get; }
    void OnEat();
}

public class Collectible : MonoBehaviour, ICollectible
{
    public CollectibleType Type => _type;

    [SerializeField] private CollectibleType _type;

    private CollectibleManager manager;

    public void init(CollectibleManager manager)
    {
        this.manager = manager;
    }

    public void OnEat()
    {
        if(manager) manager.SpawnNewItem();
        Destroy(gameObject);
    }
}