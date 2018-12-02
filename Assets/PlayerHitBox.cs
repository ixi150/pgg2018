using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    Player _owner;
    List<Collider> _hitObjects = new List<Collider>();
    bool _eatObject;

    public event Action OnSuccessfulPlayerBite = delegate { };

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
        _eatObject = false;
        Debug.Log("Clear");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_hitObjects.Contains(other)) return;
        _hitObjects.Add(other);

        //Debug.Log(other.gameObject.name);
        //Debug.Log(_eatObject);

        if (!_eatObject)
        {
            //Debug.Log("XXXXXX");
            var collectiblle = other.GetComponentInParent<ICollectible>();
            if (collectiblle != null)
            {
                _owner.eat(collectiblle);
                _eatObject = true;
            }
        }

        var player = other.GetComponentInParent<Player>();
        if (player && player != _owner)
        {
            if (GameManager.Instance.StaphActive)
            {
                _owner.ThrowUpAll();
                _owner.StunPlayer();
                if (GameManager.Instance.iSadstopEvent != null) GameManager.Instance.iSadstopEvent.Raise();
            }

            GameManager.Instance.OnPlayerBite();
            player.StunPlayer();
            player.ThrowUp();
            _owner.Vera.Trigger(_owner);
            OnSuccessfulPlayerBite();
        }
    }

//    private void OnGUI()
//    {
//        if (_owner.input.player == GamepadInput.GamePad.Index.One)
//            GUI.Label(new Rect(25, 25, 100, 25), _eatObject.ToString());
//    }
}
