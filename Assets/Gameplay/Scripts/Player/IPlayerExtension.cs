using System;
using UnityEngine;

interface IPlayerExtension
{
    void OnMoveInput(Vector2 axis);
    void OnPrimaryInput();
    void OnSecondaryInputDown();
    void OnSecondaryInputUp();

    event Action<PlayerState> StateChanged;
}

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerRuntime))]
public class PlayerExtension : MonoBehaviour, IPlayerExtension
{
    protected Player _baseScript;
    protected PlayerRuntime _runtimeScript;

    private void Awake()
    {
        _baseScript = GetComponent<Player>();
        _runtimeScript = GetComponent<PlayerRuntime>();
        _runtimeScript.RegisterExtension(this);
    }

    private void Start()
    {
        _baseScript.MoveInput += OnMoveInput;
        _baseScript.PrimaryInput += OnPrimaryInput;
        _baseScript.SecondaryInputDown += OnSecondaryInputDown;
        _baseScript.SecondaryInputUp += OnSecondaryInputUp;
    }

    private void OnDestroy()
    {
        if (_baseScript != null)
        {
            _baseScript.MoveInput -= OnMoveInput;
            _baseScript.PrimaryInput -= OnPrimaryInput;
            _baseScript.SecondaryInputDown -= OnSecondaryInputDown;
            _baseScript.SecondaryInputUp -= OnSecondaryInputUp;
        }
        if (_runtimeScript != null)
            _runtimeScript.DeregisterExtension(this);
    }

    public virtual void OnMoveInput(Vector2 axis)
    {

    }

    public virtual void OnPrimaryInput()
    {

    }

    public virtual void OnSecondaryInputDown()
    {

    }

    public virtual void OnSecondaryInputUp()
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