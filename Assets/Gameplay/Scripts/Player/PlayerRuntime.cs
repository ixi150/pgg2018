using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRuntime : MonoBehaviour
{
    public PlayerState PlayerState { get { return _playerState; } }
    private PlayerState _playerState;

    public Transform model;

    [SerializeField]
    private float _speedMultiplier;
    [SerializeField]
    private float _rotationMultiplier;
    [SerializeField]
    private Transform _playerAss, _shotPrefab;

    [SerializeField]
    private Transform _particleLoadParent, _particleShotParent;

    [SerializeField]
    private AnimationCurve _velocityDrop;

    private Transform _shotParent;

    private List<PlayerExtension> _extensions;

    private Rigidbody rigid;
    private Vector3 _lastMoveDirection;

    Player _player;

    private void Awake()
    {
        _shotParent = GameObject.Find("[BULLET_PARENT]").transform;
        _lastMoveDirection = transform.forward;
        _playerState = PlayerState.Idle;
        rigid = GetComponent<Rigidbody>();

        _player = GetComponent<Player>();
    }

    private void Update()
    {
        model.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(_lastMoveDirection, Vector3.up),
            _rotationMultiplier * Time.deltaTime);
    }

    public void RegisterExtension(PlayerExtension extension)
    {
        if (_extensions == null)
            _extensions = new List<PlayerExtension>();
        extension.StateChanged += OnStateChanged;
        extension.MoveChanged += OnMoveChanged;
        _extensions.Add(extension);
    }

    public void DeregisterExtension(PlayerExtension extension)
    {
        if (extension != null)
        {
            extension.StateChanged -= OnStateChanged;
            extension.MoveChanged -= OnMoveChanged;
        }
        if (_extensions != null)
            _extensions.Remove(extension);
    }

    private void DoStateTransition(PlayerState from, PlayerState to)
    {
        _playerState = to;
        Debug.LogWarning("PLAYER STATE CHANGED TO: " + to.ToString());
    }

    private void ShowParticles(bool activate, Transform parent)
    {
        ParticleSystem[] effects = parent.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < effects.Length; i++)
        {
            if (activate)
                effects[i].Play();
            else
                effects[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void ShotFromAss()
    {
        Transform obj = Instantiate(_shotPrefab, _playerAss.position, _playerAss.rotation);
        obj.GetComponent<FartShoot>().Init(_player, -transform.forward);
        obj.parent = _shotParent;
    }

    private void OnStateChanged(PlayerState state)
    {
        DoStateTransition(_playerState, state);
        if (state == PlayerState.StartFart)
        {
            ShowParticles(true, _particleLoadParent);
        }
        if (state == PlayerState.ReleaseFart)
        {
            ShowParticles(false, _particleLoadParent);
            ShowParticles(true, _particleShotParent);
            ShotFromAss();
        }
    }

    private void OnMoveChanged(Vector2 axis)
    {
        Vector3 realAxis = new Vector3(axis.x, 0f, axis.y);
        _lastMoveDirection = realAxis;
        rigid.MovePosition(transform.position + realAxis * _speedMultiplier * Time.deltaTime * (_velocityDrop.Evaluate(_player.eaten.Count)));
        //Debug.LogWarning("PLAYER MOVE CHANGED TO: " + axis.ToString());
    }
}

public enum PlayerState
{
    Idle,
    Move,
    CollectItem,
    AttackPlayer,
    StartFart,
    ReleaseFart,
    GiveItems
}