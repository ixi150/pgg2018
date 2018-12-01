using System;
using UnityEngine;

interface IPlayerExtension
{
    void OnMoveInput(Vector2 axis);
    void OnPrimaryInput();
    void OnSecondaryInput();

    event Action<PlayerState> StateChanged;
}

[RequireComponent(typeof(PlayerRuntime))]
public class PlayerExtension : MonoBehaviour, IPlayerExtension
{
    protected PlayerRuntime _baseScript;
    //HERE REGISTER THE INPUT MANAGER AND PLAYER
    private void Awake()
    {
        _baseScript = GetComponent<PlayerRuntime>();
        _baseScript.RegisterExtension(this);
    }

    private void OnDestroy()
    {
        if (_baseScript != null)
            _baseScript.DeregisterExtension(this);
    }
    //

    public virtual void OnMoveInput(Vector2 axis)
    {

    }

    public virtual void OnPrimaryInput()
    {

    }

    public virtual void OnSecondaryInput()
    {

    }

    public event Action<PlayerState> StateChanged;
    protected void OnStateChanged(PlayerState state)
    {
        if (StateChanged != null)
            StateChanged(state);
    }

    public event Action<Vector2> MoveChanged;
    protected void OnMoveChanged(Vector2 axis)
    {
        if (MoveChanged != null)
            MoveChanged(axis);
    }
}