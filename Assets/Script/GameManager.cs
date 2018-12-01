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

        currentTimer.Set(maxTimer);
    }

    IEnumerator ItemCollectorManager()
    {
        yield return new WaitForSeconds(10);

        while (true)
        {
            _blinking = false;
            float time = 0;

            var color = dev_item.color;
            color.a = 1;
            dev_item.color = color;

            if (_currentBonus == null)
            {
                time = bonusTime;
                _currentBonus = CollectibleTypes[Random.Range(0, CollectibleTypes.Length)];
                dev_item.enabled = true;
                yield return new WaitForSeconds(time - 3);
                _blinking = true;
                yield return new WaitForSeconds(3);
            }
            else
            {
                time = Random.Range(_currentBonus.minSpawnTime, _currentBonus.maxSpawnTime);
                _currentBonus = null;
                dev_item.enabled = false;
                //dev_item.text = (_currentBonus?.value ?? 0f).ToString();
                yield return new WaitForSeconds(time);
            }
        }
    }

    private void Update()
    {
        if (_blinking)
        {
            var color = dev_item.color;
            color.a = Mathf.PingPong(Time.time * 2, 0.8f) + 0.2f;
            dev_item.color = color;
        }

        currentTimer.Set(currentTimer - Time.deltaTime);
        timerUI.text = Mathf.FloorToInt(currentTimer).ToString();
    }



    public int[] playerPoints = new int[4];

    public void AddPoints(GamePad.Index player, Collectible[] collectable)
    {
        int points = 0;
        for (int i = 0; i < collectable.Length; i++)
        {
            points += collectable[i].Type.value * (collectable[i].Type == _currentBonus ? 3 : 1);
        }

        playerPoints[(int)player] += points;
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
