using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

[CreateAssetMenu(menuName = "Collectible")]
public class CollectibleType : ScriptableObject
{
    public GameObject View;
    public Sprite Image;
    public FloatReference minSpawnTime, maxSpawnTime;
    public IntReference value;
}
