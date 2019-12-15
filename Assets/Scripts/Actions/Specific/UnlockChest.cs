using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockChest : GameAction
{
    public string               combination;
    public GameItem             openChestItem;
    public InteractiveObject    chestObject;

    public override string GetVerb()
    {
        return "unlock";
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        // Check combination
        if (commandString.Count < 3)
        {
            player.logWindow.AddText("What's the combination?...");
            return true;
        }

        if (commandString[2] != combination.ToLower())
        {
            player.logWindow.AddText("Hum... That doesn't sound like the right combination...");
            return true;
        }

        player.logWindow.AddText("You hear a satisfactory click as the lock swings open...");

        player.gameState.SetBool("OpenChest", true);

        OpenChest();

        return true;
    }

    public override void OnLoad(PlayerController player)
    {
        base.OnLoad(player);

        if (player.gameState.GetBool("OpenChest"))
        {
            OpenChest();
        }
    }

    void OpenChest()
    {
        Animator anim = GetComponentInParent<Animator>();
        if (anim)
        {
            anim.SetTrigger("Open");
        }
        DisableAllActions();
        DisableInteractiveObject();

        chestObject.gameItem = openChestItem;
    }

}
