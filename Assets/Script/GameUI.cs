using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Xunity.ScriptableVariables;
using GamepadInput;

public class GameUI : MonoBehaviour
{
    public PointsBoard[] pointBoards = new PointsBoard[4];

    private void Start()
    {
        for (int i = 0; i < pointBoards.Length; i++)
        {
            pointBoards[i].Init((GamePad.Index)i + 1, this);
        }
    }

    public void CheckScore(PointsBoard board)
    {
        var currentFirstPlayer = pointBoards[0]._player;

        for (int i = pointBoards.Length - 2; i >= 0; i--)
        {
            if (board.points > pointBoards[i].points)
            {
                board.root.transform.SetSiblingIndex(pointBoards[i].root.transform.GetSiblingIndex());
            }
        }

        if (currentFirstPlayer != pointBoards[0]._player)
        {
            Debug.Log("X");
            pointBoards[0].PlayLeadVoice();
        }
    }

    public void PLayWinSound()
    {
        pointBoards[0].PlayLeadVoice();
    }
}

[System.Serializable]
public class PointsBoard
{
    public GameObject root;
    public FloatVariable points;
    public Text score;

    public Xunity.ScriptableEvents.GameEvent leadEvent;
    public Xunity.ScriptableEvents.GameEvent winEvent;

    GameUI _ui;
    public GamePad.Index _player;

    public void Init(GamePad.Index player, GameUI ui)
    {
        _player = player;
        _ui = ui;
        points.ValueChanged += UpdateScore;
        score.text = "Player : " + (int)_player + ": " + 0;
    }

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    public void UpdateScore(float value)
    {
        score.text = "Player : " + (int)_player + ": " + value;
        _ui.CheckScore(this);
    }

    public void PlayLeadVoice()
    {
        if (leadEvent != null)
        {
            leadEvent.Raise();
            Debug.Log("Play lead sound");
        }
    }

    public void PlayWinVoice()
    {
        if (winEvent != null) winEvent.Raise();
    }
}
