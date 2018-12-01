using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInput input;

    public string[] joystickNames;

    private void Update()
    {
        if (input.GetAxis() != Vector2.zero) Debug.Log(input.player + ": " + input.GetAxis());
        if (input.GetAttackButton()) Debug.Log(input.player + " Attack Down");
        if (input.GetShootButton()) Debug.Log("Shoot");
        if (input.GetShootButtonDown()) Debug.Log("Shoot Down");
        if (input.GetShootButtonUp()) Debug.Log("Shoot Up");

        joystickNames = Input.GetJoystickNames();
    }
}