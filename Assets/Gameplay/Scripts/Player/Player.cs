using System;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : MonoBehaviour
{
    public PlayerInput input;
    public string[] joystickNames;

    public List<Collectible> eaten;

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
        if (PrimaryInput != null)
            PrimaryInput();
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

    public void Eat(Collectible collectible)
    {
        eaten.Add(collectible);
    }

    public Collectible[] GetAllEaten()
    {
        var collectibles = eaten.ToArray();
        eaten.Clear();
        return collectibles;
    }

    public void RemoveLast()
    {
        eaten.RemoveAt(eaten.Count - 1);
    }
}