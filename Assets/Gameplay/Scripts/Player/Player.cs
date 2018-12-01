using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

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

    public new BoxCollider collider;

    PlayerRuntime _playerRuntime;
    bool _blockInput;

    private void Awake()
    {
        _playerRuntime = GetComponent<PlayerRuntime>();
    }

    private void Update()
    {
        if (_blockInput == false)
        {
            Vector2 axis = GamePad.GetAxis(GamePad.Axis.LeftStick, input.player);
            if (axis != Vector2.zero)
                OnMoveInput(axis);
            if (GamePad.GetButtonDown(GamePad.Button.A, input.player))
                OnPrimaryInput();
            if (GamePad.GetButtonDown(GamePad.Button.B, input.player))
                OnSecondaryInputDown();
            if (GamePad.GetButtonUp(GamePad.Button.B, input.player))
                OnSecondaryInputUp();
        }

        joystickNames = Input.GetJoystickNames();
    }

    public event Action<Vector2> MoveInput;
    private void OnMoveInput(Vector2 axis)
    {
        if (MoveInput != null)
            MoveInput(axis);
    }

    public event Action PrimaryInput;
    private void OnPrimaryInput()
    {
        //if (PrimaryInput != null)
        //    PrimaryInput();

        var objects = Physics.OverlapBox(collider.transform.position, collider.size);
        for (int i = 0; i < objects.Length; i++)
        {
            var collectiblle = objects[i].GetComponent<Collectible>();
            if (collectiblle != null)
            {
                eat(collectiblle);
            }

            var player = objects[i].GetComponentInParent<Player>();
            if (player && player != this)
            {
                player.StunPlayer();
            }
        }
    }

    public void ThrowUp()
    {
        if (eaten.Count <= 0) return;

        int i = UnityEngine.Random.Range(0, eaten.Count);
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

    public void eat(Collectible collectible)
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