using UnityEngine;

public class FartExtension : PlayerExtension
{
    public override void OnSecondaryInputDown()
    {
        if (_baseScript.eaten.Count > 0)
        {
            OnStateChanged(PlayerState.StartFart);
        }
    }

    public override void OnSecondaryInputUp()
    {
        if (_baseScript.eaten.Count > 0)
        {
            OnStateChanged(PlayerState.ReleaseFart);
            _baseScript.RemoveLastEaten(); 
        }
    }
}
