using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliectibleViewSpawner : MonoBehaviour
{
    protected void Start()
    {
        var collectible = GetComponent<ICollectible>();
        if (collectible == null) return;
        Instantiate(collectible.Type.Views[Random.Range(0, collectible.Type.Views.Length)], transform.Find("View"), false);
    }
}
