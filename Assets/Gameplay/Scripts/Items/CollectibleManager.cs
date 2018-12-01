using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Xunity.ScriptableVariables;

public class CollectibleManager : MonoBehaviour
{
    public FloatVariable minSpawnTimme, maxSpawnTime;
    public IntVariable maxItems;

    public GameObject collectiblePrefab;

    private Collectible _item;

    private void Start()
    {        
        SpawnItem();
    }

    public void SpawnNewItem()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnTimme, maxSpawnTime));
        SpawnItem();
    }


    void SpawnItem()
    {
        var item = Instantiate(collectiblePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Collectible>();
        item.init(this);
        _item = item;
    }
}
