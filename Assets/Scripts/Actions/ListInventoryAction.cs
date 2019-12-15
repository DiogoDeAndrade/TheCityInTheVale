using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListInventoryAction : GameAction
{
    public override string GetVerb()
    {
        return "inventory";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        player.DisplayInventory();

        return true;
    }
}
