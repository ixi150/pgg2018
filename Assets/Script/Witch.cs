using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponentInParent<Player>();
        if (player)
        {
            GameManager.Instance.AddPoints(player.input.player, player.GetAllEaten().Length);
        }
    }

}
