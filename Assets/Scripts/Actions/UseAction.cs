using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAction : GameAction
{
    public GameItem inventoryItem;

    public GameItem resultingItem;
    public string   successText;

    public override string GetVerb()
    {
        return "use";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (commandString.Count <= 1)
        {
            return false;
        }

        if (!inventoryItem.IsThisTheItem(commandString[nounIndex]))
        {
            return false;
        }

        if (!player.gameState.HasItem(inventoryItem))
        {
            return false;
        }

        if (commandString.Count <= 2)
        {
            return false;
        }

        if (!base.IsValidAction(player, commandString, 2)) return false;

        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        if (resultingItem)
        {
            player.gameState.RemoveFromInventory(inventoryItem);
            player.gameState.AddItemToInventory(resultingItem);            
        }

        if (successText != "")
        {
            player.logWindow.AddText(successText);
        }

        if ((resultingItem) || (successText != "")) return true;

        return base.RunAction(player, commandString);
    }
}
