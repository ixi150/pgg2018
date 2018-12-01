﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Xunity.ScriptableVariables;

public class FartShoot : MonoBehaviour, ICollectible
{
    public CollectibleType CollectibleType { get { return _collectibleType; } }

    [SerializeField]
    private CollectibleType _collectibleType;

    public FloatVariable forceMultiplier;
    public FloatVariable _timeToDestroy;

    bool _destroyAfterTime;

    Player _onwer;

    public void Init(Player owner, Vector3 force, bool destroyAfterTime = true)
    {
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
        if (_destroyAfterTime && other.gameObject != _onwer.gameObject)
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
