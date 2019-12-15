using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitAction : GameAction
{
    public override string GetVerb()
    {
        return "quit";
    }

    public override bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        return true;
    }

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        UnityEngine.Application.Quit();
#endif

        return true;
    }
}
