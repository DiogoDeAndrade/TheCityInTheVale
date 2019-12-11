using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction : MonoBehaviour
{
    [System.Serializable]
    public class Condition
    {
        public string condition;
        [TextArea]
        public string failText;
    }

    public bool validateObject = true;
    public bool hasConditions = false;
    [ShowIf("hasConditions")]
    public List<Condition>  conditions;

    protected InteractiveObject interactiveObject;

    protected virtual void Start()
    {
        interactiveObject = GetComponent<InteractiveObject>();
    }

    public virtual string GetVerb()
    {
        return "";
    }

    public virtual bool IsValidAction(PlayerController player, List<string> commandString, int nounIndex)
    {
        if (player.gameState.GetFloat("sanity") <= 0.0f)
        {
            return false;
        }
        if (validateObject)
        {
            if (interactiveObject.gameItem != null)
            {
                if (nounIndex >= commandString.Count)
                {
                    return false;
                }
                if (!interactiveObject.gameItem.IsThisTheItem(commandString[nounIndex]))
                {
                    if (!interactiveObject.gameItem.isContainer) return false;

                    bool found = false;
                    var contents = interactiveObject.gameItem.GetContents(player);
                    foreach (var item in contents)
                    {
                        if (item.IsThisTheItem(commandString[nounIndex]))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found) return false;
                }
            }
            foreach (var condition in conditions)
            {
                if (!player.gameState.EvaluateCondition(condition.condition))
                {
                    if (condition.failText != "")
                    {
                        player.logWindow.AddText(condition.failText);
                    }
                    return false;
                }
            }
        }

        return true;
    }

    public virtual bool RunAction(PlayerController player, List<string> commandString)
    {
        return false;
    }

    public virtual void OnLoad(PlayerController player)
    {

    }

    protected void DisableAllActions()
    {
        GameAction[] actions = GetComponentsInChildren<GameAction>();
        foreach (var action in actions)
        {
            action.enabled = false;
        }
    }

    protected void DisableInteractiveObject()
    {
        if (interactiveObject)
        {
            interactiveObject.enabled = false;
        }
    }
}
