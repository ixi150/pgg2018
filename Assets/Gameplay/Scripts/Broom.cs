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
    private Animator _animator;

    public void Hit()
    {
        _target.StunPlayer();
    }

    protected void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _vera = FindObjectOfType<Witch>();
    }

    protected void Update()
    {
        if (!_vera.InRage) Players.Clear();
        if (_target == null)
        {
            if (Players.Count > 0)
            {
                _animator.CrossFade("Follow", 0.1f);
                _target = Players[0];
                StartCoroutine(_hit(_target));
            }
            else if(transform.position != StartPoint.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, StartPoint.position, Speed * Time.deltaTime);
                if (transform.position == StartPoint.position)
                {
                    _animator.CrossFadeInFixedTime("Idle", 0.25f);
                }
            }
        }
    }

    private IEnumerator _hit(Player player)
    {
        _animator.CrossFadeInFixedTime("Follow", 0.1f);
        while (transform.position != player.transform.position)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Speed * Time.deltaTime);
        }
        _animator.Play("Hit");
        float time = 1;
        while (time > 0)
        {
            time -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Speed * Time.deltaTime);
            yield return null;
        }
        _animator.Play("Return");
        if (Players.Count > 0)
        {
            Players.RemoveAt(0);
        }
        _target = null;
    }
}
