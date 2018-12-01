using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public int[] playerPoints = new int[4];

    public void AddPoints(GamePad.Index player, int points)
    {
        playerPoints[(int)player] += points;
    }

    private void OnGUI()
    {
        for (int i = 0; i < playerPoints.Length; i++)
        {
            GUI.Label(new Rect(50, 25 + i * 25, 200, 25), "Player " + (i + 1) + ": " + playerPoints[i]);
        }
    }
}
