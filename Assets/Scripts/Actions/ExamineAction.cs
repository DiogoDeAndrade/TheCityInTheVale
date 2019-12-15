using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineAction : GameAction
{
    public enum Type { Look, Read };

    public Type type = Type.Look;

    public override string GetVerb()
    {
        return "examine";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (!base.IsValidAction(player, commandString, nounIndex)) return false;

        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        GameState gameState = player.gameState;

        // Check if it's this object
        if (interactiveObject.gameItem.IsThisTheItem(commandString[1]))
        {
            gameState.SetBool("Examined(" + interactiveObject.gameItem.itemName + ")", true);

            if (type == Type.Read)
            {
                player.Read(new List<string>() { interactiveObject.gameItem.textDescription });

                return true;
            }

            player.logWindow.AddText(interactiveObject.gameItem.textDescription);
            if ((interactiveObject.gameItem.isContainer) && (interactiveObject.gameItem.listContentsOnDescription))
            {
                var contents = interactiveObject.gameItem.GetContents(player);
                if (contents.Count == 0)
                {
                    player.logWindow.AddText("It's empty...");
                }
                else
                {
                    player.logWindow.AddText("You see inside:");
                    foreach (var item in contents)
                    {
                        player.logWindow.AddText(item.itemName);
                    }
                }
            }
        }
        else
        {
            var contents = interactiveObject.gameItem.GetContents(player);

            foreach (var item in contents)
            {
                if (item.IsThisTheItem(commandString[1]))
                {
                    gameState.SetBool("Examined(" + item.itemName + ")", true);
                    player.logWindow.AddText(item.textDescription);

                    return true;
                }
            }
        }

        return true;
    }
}
