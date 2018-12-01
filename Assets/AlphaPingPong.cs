using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class AlphaPingPong : MonoBehaviour
{
    public float minAlpha = 0.5f;
    public float blinkSpeedMultiplier = 1;

    Graphic _graphic;

    private void Awake()
    {
        _graphic = GetComponent<Graphic>();
    }

    void Update()
    {
        var color = _graphic.color;
        color.a = Mathf.PingPong(Time.time * blinkSpeedMultiplier, 1 - minAlpha) + minAlpha;
        _graphic.color = color;
    }
}
