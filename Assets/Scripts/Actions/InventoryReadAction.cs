using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryReadAction : InventoryAction
{
    public override string GetVerb()
    {
        return "read";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (base.IsValidAction(player, commandString, nounIndex))
        {
            var item = GetItem(player, commandString, nounIndex);

            if (item.item.canRead) return true;
        }

        return false;
    }

    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        var item = GetItem(player, commandString, 1);

        player.gameState.SetBool("Read(" + item.item.name + ")", true);

        player.Read(item.item.readPages);

        return true;
    }
}
