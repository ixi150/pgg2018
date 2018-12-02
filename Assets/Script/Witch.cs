using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    public float RageDegenerationSpeed = 0.1f;
    public float ActivateRageTreshold = 10;
    public float RageLength = 10;

    private float _rage;
    private float _rageEndTime;
    public bool InRage { get; private set; }

    private Broom _broom;

    protected void Awake()
    {
        _broom = FindObjectOfType<Broom>();
    }

    public void Trigger(Player player)
    {
        if (InRage)
        {
            _broom.Players.Add(player);
        }
        else
        {
            _rage += 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();
        if (player)
        {
            GameManager.Instance.AddPoints(player.input.player, player.GetAllEaten());
        }
    }

    protected void Update()
    {
        if (_rage >= ActivateRageTreshold)
        {
            _rage = 0;
            InRage = true;
            GameManager.Instance.staphObject.SetActive(true);
            _rageEndTime = Time.time + RageLength;
        }
        _rage -= RageDegenerationSpeed * Time.deltaTime;
        if (_rage < 0) _rage = 0;
        if (InRage && Time.time >= _rageEndTime)
        {
            _rage = 0;
            InRage = false;
            GameManager.Instance.staphObject.SetActive(false);
        }
    }
}
