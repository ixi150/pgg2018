using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour
{
    [NonSerialized] public List<Player> Players = new List<Player>();

    public float Speed = 5;
    public Transform StartPoint;

    private Witch _vera;
    private Player _target;

    protected void Awake()
    {
        _vera = FindObjectOfType<Witch>();
    }

    protected void Update()
    {
        if (!_vera.InRage) Players.Clear();
        if (_target == null && Players.Count > 0) _target = Players[0];
        if (_target)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Speed * Time.deltaTime);
            if (transform.position == _target.transform.position)
            {
                _target.StunPlayer();
                if (Players.Count > 0)
                {
                    Players.RemoveAt(0);
                }

                _target = null;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPoint.position, Speed * Time.deltaTime);
        }
    }
}
