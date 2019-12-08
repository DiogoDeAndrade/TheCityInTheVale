using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAction : GameAction
{
    protected GameState.InventoryItem GetItem(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (nounIndex < commandString.Count)
        {
            foreach (var item in player.gameState.inventory)
            {
                if (item.item.IsThisTheItem(commandString[nounIndex]))
                {
                    return item;
                }
            }            
        }

        return null;
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (GetItem(player, commandString, nounIndex) == null) return false;

        return base.IsValidAction(player, commandString, nounIndex);
    }
}
