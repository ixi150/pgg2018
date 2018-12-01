using System;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : MonoBehaviour
{
    public PlayerInput input;
    public string[] joystickNames;

    public List<CollectibleType> eaten;

    public new BoxCollider collider;

    private void Update()
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

        var items = Physics.OverlapBox(collider.transform.position, collider.size);
        for (int i = 0; i < items.Length; i++)
        {
            var collectiblle = items[i].GetComponent<Collectible>();
            if (collectiblle)
            {
                eat(collectiblle);
            }
        }
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
        eaten.Add(collectible.CollectibleType);
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
}