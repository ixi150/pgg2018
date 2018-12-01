using UnityEngine;

public class CollectExtension : PlayerExtension
{
    private PlayerState GetTargetStateChange()
    {
        return PlayerState.CollectItem;
    }

    public override void OnPrimaryInput()
    {
        OnStateChanged(GetTargetStateChange());
    }
}
