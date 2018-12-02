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
    [SerializeField]
    private GameObject _particlesOnDestroy;

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
        if (manager)
        {
            manager.Items--;
            manager.SpawnNewItem();
        }

        if (_particlesOnDestroy != null)
            Instantiate(_particlesOnDestroy, transform.position, _particlesOnDestroy.transform.rotation);
        Destroy(gameObject);
    }
}