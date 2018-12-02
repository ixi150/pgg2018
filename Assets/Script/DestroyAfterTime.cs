using System.Collections;
using UnityEngine;

using Xunity.ScriptableVariables;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private FloatVariable _timeToDestroy;
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Destroy(gameObject);
    }
}
