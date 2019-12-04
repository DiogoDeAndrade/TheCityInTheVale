using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAction : GameAction
{
    public string filename = "save.json";

    public override string GetVerb()
    {
        return "save";
    }

    public override bool IsValidAction(PlayerController player, string[] commandString, int nounIndex)
    {
        return true;
    }

    public override bool RunAction(PlayerController player)
    {
        GameState   gameState = player.gameState;
        string      json = JsonUtility.ToJson(gameState);

        player.outputWindow.AddText("Saved " + filename);

        System.IO.File.WriteAllText(filename, json);

        return true;
    }
}
