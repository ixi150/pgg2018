using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Xunity.ScriptableVariables;

public class CollectibleManager : MonoBehaviour
{
    public IntVariable maxItems;

    public Collectible collectiblePrefab;

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
        yield return new WaitForSeconds(Random.Range(collectiblePrefab.Type.minSpawnTime, collectiblePrefab.Type.maxSpawnTime));
        SpawnItem();
    }


    void SpawnItem()
    {
        var item = Instantiate(collectiblePrefab, transform.position, Quaternion.identity).GetComponent<Collectible>();
        item.init(this);
        item.GetComponent<Animator>().Play("Spawn");
        _item = item;
    }
}
