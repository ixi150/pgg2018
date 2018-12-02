using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xunity.ScriptableEvents;

public class EventRaiser : MonoBehaviour
{
    [SerializeField] GameEvent[] enabledEvents, disabledEvents;

    void OnEnable()
    {
        foreach (var gameEvent in enabledEvents)
        {
            gameEvent.Raise();
        }
    }

    void OnDisable()
    {
        foreach (var gameEvent in disabledEvents)
        {
            gameEvent.Raise();
        }
    }
}