using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Xunity.ScriptableVariables;

public class FartShoot : MonoBehaviour
{
    public FloatVariable forceMultiplier;

    Player _onwer;

    public void Init(Player owner, Vector3 force)
    {
        GetComponent<Rigidbody>().velocity = force * forceMultiplier;
        _onwer = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != _onwer.gameObject)
        {
            var player = other.GetComponentInParent<Player>();
            if (player == true)
            {
                player.StunPlayer();
            }

            Destroy(gameObject);
        }
    }
}
