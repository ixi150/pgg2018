using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliectibleViewSpawner : MonoBehaviour
{
    protected void Start()
    {
        var collectible = GetComponent<ICollectible>();
        if (collectible == null) return;
        Instantiate(collectible.Type.View, transform, false);
    }
}
