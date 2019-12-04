using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadAction : GameAction
{
    public string filename = "save.json";

    public override string GetVerb()
    {
        return "load";
    }

    public override bool IsValidAction(PlayerController player, string[] commandString, int nounIndex)
    {
        return true;
    }

    public override bool RunAction(PlayerController player)
    {
        if (!System.IO.File.Exists(filename))
        {
            player.outputWindow.AddText("Can't open file " + filename);
            return false;
        }

        PlayerController.loadFile = filename;

        SceneManager.LoadScene("GameScene");

        return true;
    }
}
