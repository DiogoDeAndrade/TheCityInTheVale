﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryAction : GameAction
{
    public override string GetVerb()
    {
        return "inventory";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        return true;
    }

    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        player.DisplayInventory();

        return true;
    }
}
