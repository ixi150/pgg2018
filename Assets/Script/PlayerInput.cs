using UnityEngine;

public enum PlayerID
{
    player_1 = 1,
    player_2 = 2,
    player_3 = 3,
    player_4 = 4,
}

[CreateAssetMenu(fileName = "Input", menuName = "Player/Input", order = 0)]
public class PlayerInput : ScriptableObject
{
    public int attackID = 0, shootID = 1;

    public PlayerID player;

    public Vector2 GetAxis()
    {
        var vector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        return vector;
    }

    public bool GetAttackButton()
    {
        return Input.GetKey("joystick " + (int)player + " button " + attackID);
    }

    public bool GetShootButton()
    {
        return Input.GetKey("joystick " + (int)player + " button " + shootID);
    }

    public bool GetShootButtonDown()
    {
        return Input.GetKeyDown("joystick " + (int)player + " button " + shootID);
    }

    public bool GetShootButtonUp()
    {
        return Input.GetKeyUp("joystick " + (int)player + " button " + shootID);
    }
}