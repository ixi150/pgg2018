using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerRuntime : MonoBehaviour
{
    public PlayerState PlayerState { get { return _playerState; } }
    private PlayerState _playerState;

    [SerializeField]
    private float _speedMultiplier;
    [SerializeField]
    private float _rotationMultiplier;

    private List<PlayerExtension> _extensions;

    private Rigidbody rigid;
    private Vector3 _lastMoveDirection;

    private void Awake()
    {
        _lastMoveDirection = transform.forward;
        _playerState = PlayerState.Idle;
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, 
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

    private void OnStateChanged(PlayerState state)
    {
        DoStateTransition(_playerState, state);
    }

    private void OnMoveChanged(Vector2 axis)
    {
        Vector3 realAxis = new Vector3(axis.x, 0f, axis.y);
        _lastMoveDirection = realAxis;
        rigid.MovePosition(transform.position + realAxis * _speedMultiplier * Time.deltaTime);
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