using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broom : MonoBehaviour
{
    [NonSerialized] public List<Player> Players = new List<Player>();

    public float Speed = 5;
    public Transform StartPoint;

    protected void Update()
    {
        if (Players.Count == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPoint.position, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, Players[0].transform.position, Speed * Time.deltaTime);
            if (transform.position == Players[0].transform.position)
            {
                Players[0].StunPlayer();
                Players.RemoveAt(0);
            }
        }
    }
}
