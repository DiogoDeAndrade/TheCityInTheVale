using System.Collections;
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

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (!base.IsValidAction(player, commandString, nounIndex)) return false;

        return true;
    }
    
    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        GameState gameState = player.gameState;

        gameState.AddItemToInventory(interactiveObject.gameItem);
        gameState.SetBool("PickedUp(" + interactiveObject.gameItem.itemName + ")", true);

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

        if (gameState.GetBool("PickedUp(" + interactiveObject.gameItem.itemName + ")"))
        {
            Destroy(interactiveObject.gameObject);
        }
    }
}
