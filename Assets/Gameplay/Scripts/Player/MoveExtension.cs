using UnityEngine;

public class MoveExtension : PlayerExtension
{
    public override void OnMoveInput(Vector2 axis)
    {
        OnMoveChanged(axis);
    }
}
