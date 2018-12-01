using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
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
        yield return new WaitForSeconds(1);
        SpawnItem();
    }


    void SpawnItem()
    {
        var item = Instantiate(collectiblePrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity).GetComponent<Collectible>();
        item.init(this);
        _item = item;
    }
}
