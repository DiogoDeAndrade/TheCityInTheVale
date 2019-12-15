using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineAction : GameAction
{
    public GameItem item1;
    public GameItem item2;

    public GameItem[] resultingItems;

    [TextArea]
    public string successText;

    public override string GetVerb()
    {
        return "use";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (commandString.Count < 3) return false;

        if (!player.gameState.HasItem(item1)) return false;
        if (!player.gameState.HasItem(item2)) return false;

        if (((item1.IsThisTheItem(commandString[1])) && (item2.IsThisTheItem(commandString[2]))) ||
            ((item2.IsThisTheItem(commandString[1])) && (item1.IsThisTheItem(commandString[2]))))
        {
            return base.IsValidAction(player, commandString, nounIndex);
        }

        return false;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        if (!player.gameState.HasItem(item1)) return false;
        if (!player.gameState.HasItem(item2)) return false;

        player.gameState.RemoveFromInventory(item1);
        player.gameState.RemoveFromInventory(item2);

        foreach (var resultingItem in resultingItems)
        {
            player.gameState.AddItemToInventory(resultingItem);
        }

        player.logWindow.AddText(successText);

        return true;
    }
}
