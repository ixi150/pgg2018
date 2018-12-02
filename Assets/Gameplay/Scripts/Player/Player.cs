using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using UnityEngine.UI;

using Xunity.ScriptableVariables;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public FloatVariable points;
    public PlayerInput input;
    public string[] joystickNames;

    public float throwUpOffset = 0.5f;
    public Vector3 minThrowUpPower;
    public Vector3 maxThrowUpPower;
    
    public List<CollectibleType> eaten = new List<CollectibleType>();

    //public new BoxCollider collider;

    public Text playerNumber;

    private Animator _animator;
    private PlayerRuntime _playerRuntime;
    private bool _blockInput;
    public Witch Vera { get; private set; }

    private void Awake()
    {
        Vera = FindObjectOfType<Witch>();
        _playerRuntime = GetComponent<PlayerRuntime>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _playerHitBox = GetComponentInChildren<PlayerHitBox>();
        playerNumber.text = "P" + (int)input.player;
    }

    private void Update()
    {
        if (_blockInput == false && !_animator.GetCurrentAnimatorStateInfo(0).IsTag("Stun"))
        {
            Vector2 axis = GamePad.GetAxis(GamePad.Axis.LeftStick, input.player);
            OnMoveInput(axis);

            if (GamePad.GetButtonDown(GamePad.Button.A, input.player))
                OnPrimaryInput();
            if (GamePad.GetButtonDown(GamePad.Button.B, input.player))
                OnSecondaryInputDown();
            if (GamePad.GetButtonUp(GamePad.Button.B, input.player))
                OnSecondaryInputUp();
        }
        else
        {
            _animator.SetFloat("Move", 0);
        }

        joystickNames = Input.GetJoystickNames();
    }

    public event Action<Vector2> MoveInput;
    private void OnMoveInput(Vector2 axis)
    {
        _animator.SetFloat("Move", Mathf.Abs(axis.magnitude));

        if (axis == Vector2.zero) return;

        if (MoveInput != null)
            MoveInput(axis);
    }

    public event Action PrimaryInput;

    PlayerHitBox _playerHitBox;
    private void OnPrimaryInput()
    {
        _animator.Play("Eat");
        _playerHitBox.CleatHitBox();
    }

    public void ResetTriggers()
    {
        _animator.Play("Idle", 0, 0);
        _animator.ResetTrigger("Eat");
        _animator.ResetTrigger("Stun");
    }

    public void ThrowUp()
    {
        if (eaten.Count <= 0) return;

        int i = Random.Range(0, eaten.Count);
        var item = eaten[i];
        eaten.RemoveAt(i);
        Vector3 power = new Vector3();
        power.x = Random.Range(minThrowUpPower.x, maxThrowUpPower.x);
        power.y = Random.Range(minThrowUpPower.y, maxThrowUpPower.y);
        power.z = Random.Range(minThrowUpPower.z, maxThrowUpPower.z);
        var throwed = Instantiate(item.Prefab, transform.position + power.normalized * throwUpOffset, Quaternion.identity);
        throwed.GetComponent<Rigidbody>().AddForce(power);
    }

    public void ThrowUp(int count)
    {
        for (int i = 0; i < count; i++)
        {
            ThrowUp();
        }
    }

    public void ThrowUpAll()
    {
        ThrowUp(eaten.Count);
    }

    public event Action SecondaryInputDown;
    private void OnSecondaryInputDown()
    {
        if (SecondaryInputDown != null)
            SecondaryInputDown();
    }

    public event Action SecondaryInputUp;
    private void OnSecondaryInputUp()
    {
        if (SecondaryInputUp != null)
            SecondaryInputUp();
    }

    public void eat(ICollectible collectible)
    {
        eaten.Add(collectible.Type);
        collectible.OnEat();
    }

    public CollectibleType[] GetAllEaten()
    {
        var collectibles = eaten.ToArray();
        eaten.Clear();
        return collectibles;
    }


    public void RemoveLastEaten()
    {
        eaten.RemoveAt(eaten.Count - 1);
    }

    public void StunPlayer()
    {
        if (_blockInput == true) return;

        if (_blockInputCoroutine != null) StopCoroutine(_blockInputCoroutine);
        _blockInputCoroutine = StartCoroutine(BlockInput());
        _animator.SetTrigger("Stun");
    }

    private Coroutine _blockInputCoroutine;
    IEnumerator BlockInput()
    {
        _blockInput = true;
        yield return new WaitForSeconds(2);
        _blockInput = false;
        _blockInputCoroutine = null;
    }
}