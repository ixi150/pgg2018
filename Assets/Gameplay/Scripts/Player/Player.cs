using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput input;
    public string[] joystickNames;

    private void Update()
    {
        if (input.GetAxis() != Vector2.zero)
            OnMoveInput(input.GetAxis());
        if (input.GetAttackButton())
            OnPrimaryInput();
        if (input.GetShootButtonDown())
            OnSecondaryInputDown();
        if (input.GetShootButtonUp())
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
}