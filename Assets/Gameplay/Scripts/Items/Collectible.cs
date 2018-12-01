using System;
using System.Collections;
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

    protected void Awake()
    {
        GetComponentInChildren<Collider>().enabled = false;
    }

    protected IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);

        GetComponentInChildren<Collider>().enabled = true;
    }

    public void init(CollectibleManager manager)
    {
        this.manager = manager;
        GetComponentInChildren<Collider>().enabled = true;
    }

    public void OnEat()
    {
        if(manager) manager.SpawnNewItem();
        Destroy(gameObject);
    }
}