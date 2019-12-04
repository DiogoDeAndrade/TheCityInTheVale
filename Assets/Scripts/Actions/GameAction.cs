using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction : MonoBehaviour
{
    protected InteractiveObject interactiveObject;

    protected virtual void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
    }

    public virtual string GetVerb()
    {
        return "";
    }

    public virtual bool IsValidAction(PlayerController player, string[] commandString, int nounIndex)
    {
        if (interactiveObject.gameItem != null)
        {
            if (nounIndex >= commandString.Length)
            {
                return false;
            }
            if (!interactiveObject.gameItem.IsThisTheItem(commandString[nounIndex]))
            {
                return false;
            }
        }

        return true;
    }

    public virtual bool RunAction(PlayerController player)
    {
        return false;
    }

    public virtual void OnLoad(PlayerController player)
    {

    }
}
