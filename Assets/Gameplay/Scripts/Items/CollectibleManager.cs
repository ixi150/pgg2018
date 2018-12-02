using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Xunity.ScriptableVariables;

public class CollectibleManager : MonoBehaviour
{
    public IntVariable maxItems;
    public Collectible collectiblePrefab;
    public Vector2 SpawnArea = new Vector2(2, 2);

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
        var pos = transform.position;
        pos.x += Random.Range(-SpawnArea.x, SpawnArea.x);
        pos.z += Random.Range(-SpawnArea.y, SpawnArea.y);
        _items++;
        var item = Instantiate(collectiblePrefab, pos, Quaternion.identity).GetComponent<Collectible>();
        item.init(this);
        item.GetComponent<Animator>().Play("Spawn");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.25f);
        Gizmos.DrawCube(transform.position, new Vector3(SpawnArea.x, 0 , SpawnArea.y));
    }


}
