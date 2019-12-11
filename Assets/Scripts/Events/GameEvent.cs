using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public enum Type { OnEnter, OnStay, OnExit };

    public Type type;

    private void OnTriggerEnter(Collider other)
    {
        if (type != Type.OnEnter) return;

        var player = GetPlayer(other);

        if (player != null) RunEvent(player);
    }

    private void OnTriggerStay(Collider other)
    {
        if (type != Type.OnStay) return;

        var player = GetPlayer(other);

        if (player != null) RunEvent(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (type != Type.OnExit) return;

        var player = GetPlayer(other);

        if (player != null) RunEvent(player);
    }

    PlayerController GetPlayer(Collider other)
    { 
        var player = other.GetComponent<PlayerController>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerController>();

            if (player == null) return null;
        }

        return player;
    }

    protected virtual void RunEvent(PlayerController player)
    {

    }
}
