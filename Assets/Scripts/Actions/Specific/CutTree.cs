using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTree : UseAction
{
    public GameItem             fallenTree;
    public InteractiveObject    treeObject;

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        player.logWindow.AddText("You use the axe to chop down the tree...");

        player.gameState.SetBool("FellTree", true);

        ChopTree();

        return true;
    }

    public override void OnLoad(PlayerController player)
    {
        base.OnLoad(player);

        if (player.gameState.GetBool("FellTree"))
        {
            ChopTree();
        }
    }


    void ChopTree()
    {
        enabled = false;
        treeObject.gameItem = fallenTree;

        transform.rotation = Quaternion.Euler(0, 0, -86);
    }
}
