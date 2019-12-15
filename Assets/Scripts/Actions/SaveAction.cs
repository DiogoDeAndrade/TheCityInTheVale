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

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        GameState   gameState = player.gameState;
        string      json = JsonUtility.ToJson(gameState);

        player.logWindow.AddText("Saved " + filename);

        System.IO.File.WriteAllText(filename, json);

        return true;
    }
}
