﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAction : GameAction
{
    public bool     destroyOnPickup = true;
    [TextArea]
    public string   pickupResponse;

    public override string GetVerb()
    {
        return "pickup";
    }

    public override bool IsValidAction(PlayerController player, string[] commandString, int nounIndex)
    {
        if (!base.IsValidAction(player, commandString, nounIndex)) return false;

        return true;
    }

    public override bool RunAction(PlayerController player)
    {
        GameState gameState = player.gameState;

        gameState.AddItemToInventory(interactiveObject.gameItem);
        gameState.SetBool("Pickup" + interactiveObject.gameItem.itemName, true);

        player.outputWindow.AddText(pickupResponse);

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        return true;
    }

    public override void OnLoad(PlayerController player)
    {
        base.OnLoad(player);

        GameState gameState = player.gameState;

        if (gameState.GetBool("Pickup" + interactiveObject.gameItem.itemName))
        {
            Destroy(interactiveObject.gameObject);
        }
    }
}
