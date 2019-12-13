using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GameEvent : MonoBehaviour
{
    public enum Type { OnEnter, OnStay, OnExit };

    public Type     type;
    public bool     oneShot = false;
    public float    cooldown = 0.0f;
    [ShowIf("oneShot")]
    public string   eventName;

    float lastTime = -float.MaxValue;

    private void OnTriggerEnter(Collider other)
    {
        if (type != Type.OnEnter) return;

        var player = GetPlayer(other);

        TriggerEvent(player);
    }

    private void OnTriggerStay(Collider other)
    {
        if (type != Type.OnStay) return;

        var player = GetPlayer(other);

        TriggerEvent(player);
    }

    private void OnTriggerExit(Collider other)
    {
        if (type != Type.OnExit) return;

        var player = GetPlayer(other);

        TriggerEvent(player);
    }

    void TriggerEvent(PlayerController player)
    {
        if (player == null) return;

        if ((eventName != "") && (oneShot))
        {
            if (player.gameState.GetBool(eventName))
            {
                enabled = false;
                return;
            }
        }

        if (cooldown > 0.0f)
        {
            if ((Time.time - lastTime) < cooldown) return;

            lastTime = Time.time;
        }

        if (eventName != "")
        {
            player.gameState.SetBool(eventName, true);
        }

        RunEvent(player);

        if (oneShot) enabled = false;
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
