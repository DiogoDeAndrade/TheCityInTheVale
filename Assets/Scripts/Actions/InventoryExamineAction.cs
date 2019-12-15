﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryExamineAction : GameAction
{
    public override string GetVerb()
    {
        return "examine";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (commandString.Count < 2) return false;

        if (!player.gameState.HasItem(commandString[nounIndex])) return false;

        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        if (commandString[1] == "inventory")
        {
            player.DisplayInventory();
            return true;
        }

        foreach (var item in player.gameState.inventory)
        {
            if (item.item.IsThisTheItem(commandString[1]))
            {
                player.logWindow.AddText(item.item.textDescription);
                return true;
            }
        }

        return false;
    }
}
