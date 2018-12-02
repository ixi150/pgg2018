using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    Player _owner;
    List<Collider> _hitObjects = new List<Collider>();

    public event Action OnSuccessfulPlayerBite = delegate {  }; 
    
    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
    }

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    public void ClearHitBox()
    {
        _hitObjects.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hitObjects.Contains(other)) return;
        _hitObjects.Add(other);

        var collectiblle = other.GetComponentInParent<ICollectible>();
        if (collectiblle != null)
        {
            _owner.eat(collectiblle);
        }

        var player = other.GetComponentInParent<Player>();
        if (player && player != _owner)
        {
            if (GameManager.Instance.StaphActive)
            {
                _owner.ThrowUpAll();
                _owner.StunPlayer();
            }

            GameManager.Instance.OnPlayerBite();
            player.StunPlayer();
            player.ThrowUp();
            _owner.Vera.Trigger(_owner);
            OnSuccessfulPlayerBite();
        }
    }
}
