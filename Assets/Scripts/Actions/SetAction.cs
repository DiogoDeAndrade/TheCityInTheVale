using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAction : GameAction
{
    public override string GetVerb()
    {
        return "set";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (commandString.Count < 2) return false;

        return true;
    }

    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        string value = commandString[2];

        if (value == "true") player.gameState.SetBool(commandString[1], true);
        else if (value == "false") player.gameState.SetBool(commandString[1], false);
        else
        {
            int val;
            if (int.TryParse(commandString[2], out val))
            {
                player.gameState.SetInt(commandString[1], val);
            }
            else
            {
                player.gameState.SetInt(commandString[1], 0);
            }
        }

        return true;
    }
}
