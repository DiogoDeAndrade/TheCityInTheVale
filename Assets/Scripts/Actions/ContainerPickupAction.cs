using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerPickupAction : GameAction
{
    public override string GetVerb()
    {
        return "pickup";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (!base.IsValidAction(player, commandString, nounIndex)) return false;

        if (interactiveObject)
        {
            if (interactiveObject.gameItem)
            {
                if (interactiveObject.gameItem.IsThisTheItem(commandString[nounIndex]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        GameState gameState = player.gameState;

        var contents = interactiveObject.gameItem.GetContents(player);

        foreach (var item in contents)
        {
            if (item.IsThisTheItem(commandString[1]))
            {
                gameState.AddItemToInventory(item);
                gameState.SetBool("PickedUp(" + item.itemName + ")", true);
                player.logWindow.AddText("You pickup " + commandString[1]);

                return true;
            }
        }

        return true;
    }
}
