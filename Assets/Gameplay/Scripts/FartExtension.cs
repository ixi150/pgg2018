using UnityEngine;

public class FartExtension : PlayerExtension
{
    public override void OnSecondaryInput()
    {
        OnStateChanged(PlayerState.Fart);
    }
}
