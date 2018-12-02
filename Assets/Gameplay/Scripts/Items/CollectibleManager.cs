using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Xunity.ScriptableVariables;

public class CollectibleManager : MonoBehaviour
{
    public IntVariable maxItems;
    public Collectible collectiblePrefab;

    private int _items;
    private Coroutine _coroutine;
    
    private void Start()
    {
        SpawnNewItem();
    }

    public void SpawnNewItem()
    {
        if (_coroutine != null) return;

        _coroutine = StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (_items < maxItems)
        {
            yield return new WaitForSeconds(Random.Range(collectiblePrefab.Type.minSpawnTime, collectiblePrefab.Type.maxSpawnTime));
            SpawnItem();
        }

        _coroutine = null;
    }


    void SpawnItem()
    {
        _items++;
        var item = Instantiate(collectiblePrefab, transform.position, Quaternion.identity).GetComponent<Collectible>();
        item.init(this);
        item.GetComponent<Animator>().Play("Spawn");
    }
}
