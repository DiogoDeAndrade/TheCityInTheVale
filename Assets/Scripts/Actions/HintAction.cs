using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class HintAction : GameAction
{
    public string verb = "hint";
    public string hint;
    public bool   oneShot = false;
    [ShowIf("oneShot")]
    public string var;

    public override string GetVerb()
    {
        return verb;
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (interactiveObject)
        {
            if (interactiveObject.gameItem)
            {
                if (nounIndex < commandString.Count)
                {
                    if (interactiveObject.gameItem.IsThisTheItem(commandString[nounIndex]))
                    {
                        if ((oneShot) && (var != ""))
                        {
                            string hintVar = "Hint(" + var + ")";
                            if (player.gameState.GetBool(hintVar))
                            {
                                return false;
                            }
                        }

                        return true;
                    }

                    return false;
                }
            }
        }

        return base.IsValidAction(player, commandString, nounIndex);
    }

    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        if (var != "")
        {
            string hintVar = "Hint(" + var + ")";

            player.gameState.SetBool(hintVar, true);
        }

        player.logWindow.AddText(hint);

        return true;
    }
}
