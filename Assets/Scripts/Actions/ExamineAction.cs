using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineAction : GameAction
{
    public override string GetVerb()
    {
        return "examine";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (!base.IsValidAction(player, commandString, nounIndex)) return false;

        return true;
    }

    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        GameState gameState = player.gameState;

        gameState.SetBool("Examine" + interactiveObject.gameItem.itemName, true);

        player.outputWindow.AddText(interactiveObject.gameItem.textDescription);

        return true;
    }
}
