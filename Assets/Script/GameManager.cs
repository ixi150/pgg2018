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

    public Image itemIcon;
    public Sprite[] icons;
    public Text dev_item;


    [MinMaxRange(20, 30)]
    public RangedFloat timeToNextBonus;
    public FloatVariable bonusTime;
    public IntVariable requestedMultiplier;

    public CollectibleType[] CollectibleTypes;

    public static GameManager Instance;

    CollectibleType _currentBonus;
    bool _blinking;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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
                dev_item.enabled = true;
                if (itemIcon) itemIcon.enabled = true;
                yield return new WaitForSeconds(time - 3);
                _blinking = true;
                yield return new WaitForSeconds(3);
            }
            else
            {
                time = Random.Range(_currentBonus.minSpawnTime, _currentBonus.maxSpawnTime);
                _currentBonus = null;
                dev_item.enabled = false;
                if (itemIcon) itemIcon.enabled = false;
                yield return new WaitForSeconds(time);
            }
        }
    }

    private void Update()
    {
        if (_blinking && itemIcon)
        {
            var color = itemIcon.color;
            color.a = Mathf.PingPong(Time.time * 2, 0.8f) + 0.2f;
            dev_item.color = color;
            itemIcon.color = color;
        }

        currentTimer.Set(currentTimer - Time.deltaTime);
        timerUI.text = Mathf.FloorToInt(currentTimer).ToString();

        if (currentTimer <= 0)
        {
            Debug.Log("GAME END");
        }
    }

    public FloatVariable[] playerPoints = new FloatVariable[4];

    public void AddPoints(GamePad.Index player, CollectibleType[] collectable)
    {
        int points = 0;
        for (int i = 0; i < collectable.Length; i++)
        {
            points += collectable[i].value * (collectable[i] == _currentBonus ? 3 : 1);
        }

        playerPoints[(int)player - 1].Set(playerPoints[(int)player - 1] + points);
    }

    public float GetRandomTime()
    {
        return Random.Range(timeToNextBonus.minValue, timeToNextBonus.maxValue);
    }

    private void OnGUI()
    {
        for (int i = 0; i < playerPoints.Length; i++)
        {
            GUI.Label(new Rect(50, 25 + i * 25, 200, 25), "Player " + (i + 1) + ": " + playerPoints[i]);
        }
    }
}
