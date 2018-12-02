using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

using Xunity.ScriptableVariables;

public class GameManager : MonoBehaviour
{
    public FloatVariable currentTimer, maxTimer;
    public Text timerUI;

    public GameUI ui;

    public GameObject staphObject;

    public Image itemIcon;
    public Text dev_item;

    public FloatVariable biteTimeOffset;
    public FloatVariable staphTime;
    public IntVariable maxBitePerTimeOffset;
    public List<float> bites;

    [MinMaxRange(20, 30)]
    public RangedFloat timeToNextBonus;
    public FloatVariable bonusTime;
    public IntVariable requestedMultiplier;

    public CollectibleType[] CollectibleTypes;

    public static GameManager Instance;

    public Xunity.ScriptableEvents.GameEvent dontFightEvent;
    public Xunity.ScriptableEvents.GameEvent iSadstopEvent;

    public AudioPlayer playerGave, playerGaveBonus;
    
    public bool StaphActive { get; private set; }

    CollectibleType _currentBonus;
    bool _blinking;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        staphObject.SetActive(false);
        _currentBonus = null;
        StartCoroutine(ItemCollectorManager());
        dev_item.enabled = false;
        if (itemIcon) itemIcon.enabled = false;

        currentTimer.Set(maxTimer);

        for (int i = 0; i < playerPoints.Length; i++)
        {
            playerPoints[i].Set(0);
        }
    }

    IEnumerator ItemCollectorManager()
    {
        yield return new WaitForSeconds(10);

        while (true)
        {
            _blinking = false;
            float time = 0;

            if (itemIcon)
            {
                var color = itemIcon.color;
                color.a = 1;
                dev_item.color = color;
                itemIcon.color = color;
            }

            if (_currentBonus == null)
            {
                time = bonusTime;

                _currentBonus = CollectibleTypes[Random.Range(0, CollectibleTypes.Length)];
                dev_item.text = _currentBonus.ToString();

                _currentBonus = CollectibleTypes[Random.Range(0, CollectibleTypes.Length)];
                _currentBonus.OnIsDemandedEvent.Raise();
                itemIcon.sprite = _currentBonus.Image;
                if (itemIcon) itemIcon.enabled = true;
                yield return new WaitForSeconds(time - 3);
                _blinking = true;
                yield return new WaitForSeconds(3);
            }
            else
            {
                time = Random.Range(_currentBonus.minSpawnTime, _currentBonus.maxSpawnTime);
                _currentBonus = null;
                if (itemIcon) itemIcon.enabled = false;
                yield return new WaitForSeconds(time);
            }
        }
    }

    bool _gameEnd;
    private void Update()
    {
        if (_gameEnd) return;

        if (_blinking && itemIcon)
        {
            var color = itemIcon.color;
            color.a = Mathf.PingPong(Time.time * 2, 0.8f) + 0.2f;
            itemIcon.color = color;
        }

        currentTimer.Set(currentTimer - Time.deltaTime);
        timerUI.text = Mathf.FloorToInt(currentTimer).ToString();

        if (currentTimer <= 0)
        {
            _gameEnd = true;
            ui.PLayWinSound();
            timerUI.gameObject.SetActive(false);
            Debug.Log("GAME END");
        }
    }

    public FloatVariable[] playerPoints = new FloatVariable[4];

    public void AddPoints(GamePad.Index player, CollectibleType[] collectable)
    {
        int points = 0;
        bool anyBonus = false;
        for (int i = 0; i < collectable.Length; i++)
        {
            bool isBonus = collectable[i] == _currentBonus;
            points += collectable[i].value * (isBonus ? 3 : 1);
            anyBonus |= isBonus;
        }

        playerPoints[(int)player - 1].Set(playerPoints[(int)player - 1] + points);
        playerGave.Play();
        if(anyBonus)
            playerGaveBonus.Play();
    }

    public float GetRandomTime()
    {
        return Random.Range(timeToNextBonus.minValue, timeToNextBonus.maxValue);
    }

    public void OnPlayerBite()
    {
        if (StaphActive) return;

        bites.Add(Time.realtimeSinceStartup);

        if (bites.Count > 1)
        {
            if (bites[bites.Count - 1] - bites[0] < biteTimeOffset && bites.Count > maxBitePerTimeOffset)
            {
                bites.Clear();
                StartCoroutine(WaitForStaphEnd());
                return;
            }

            for (int i = bites.Count - 1; i >= 0; i--)
            {
                if (Time.realtimeSinceStartup > bites[i] + biteTimeOffset)
                    bites.RemoveAt(i);
            }
        }
    }

    IEnumerator WaitForStaphEnd()
    {
        if (dontFightEvent != null) dontFightEvent.Raise();

        staphObject.SetActive(true);
        StaphActive = true;
        yield return new WaitForSeconds(staphTime);
        staphObject.SetActive(false);
        StaphActive = false;
    }
}
