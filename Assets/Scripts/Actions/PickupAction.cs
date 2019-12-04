using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAction : GameAction
{
    public bool destroyOnPickup = true;

    public override string GetVerb()
    {
        return "pickup";
    }

    public override bool IsValidAction(GameState gameState, string[] commandString, int nounIndex)
    {
        if (!base.IsValidAction(gameState, commandString, nounIndex)) return false;

        return true;
    }

    public override bool RunAction(GameState gameState)
    {
        gameState.AddItemToInventory(interactiveObject.gameItem);
        gameState.SetBool("Pickup" + interactiveObject.gameItem.itemName, true);

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        return true;
    }  
}
 