using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartAction : GameAction
{
    public override string GetVerb()
    {
        return "restart";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        PlayerController.loadFile = "";

        SceneManager.LoadScene("GameScene");

        return true;
    }
}
