using UnityEngine;

public class FartExtension : PlayerExtension
{
    public override void OnSecondaryInputDown()
    {
        OnStateChanged(PlayerState.StartFart);
    }

    public override void OnSecondaryInputUp()
    {
        OnStateChanged(PlayerState.ReleaseFart);
    }
}
