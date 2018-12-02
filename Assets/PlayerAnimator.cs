using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Player _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<Player>();
    }

    public void ResetTrigger()
    {
        _owner.ResetTriggers();
    }

    public void MoveStep()
    {
        _owner.PlayStepSound();
    }

    public void PlayBite()
    {
        _owner.PlayTryBiteSound();
    }
}
