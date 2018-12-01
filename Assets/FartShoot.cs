using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Xunity.ScriptableVariables;

public class FartShoot : MonoBehaviour, ICollectible
{
    public CollectibleType Type { get; set; }

    public FloatVariable forceMultiplier;
    public FloatVariable _timeToDestroy;

    bool _destroyAfterTime;

    Player _onwer;

    public void Init(Player owner, Vector3 force, bool destroyAfterTime = true)
    {
        Type = owner.eaten.LastOrDefault();
        _destroyAfterTime = destroyAfterTime;
        if (_destroyAfterTime)
        {
            StartCoroutine(DestroyAfterTime());
        }
        GetComponent<Rigidbody>().velocity = force * forceMultiplier;
        _onwer = owner;
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_timeToDestroy);
        Destroy(gameObject);
    }

    public void OnEat()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger && _destroyAfterTime && other.GetComponentInParent<Player>() != _onwer)
        {
            var player = other.GetComponentInParent<Player>();
            if (player == true)
            {
                player.StunPlayer();
            }

            Destroy(gameObject);
        }
    }
}
