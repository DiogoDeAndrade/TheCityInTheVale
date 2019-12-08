using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "Game/Item")]
public class GameItem : ScriptableObject
{
    public string   itemName;
    public string[] synonyms;
    public string   onScreenDescription;
    [TextArea]
    public string           textDescription;
    public bool             listContentsOnDescription;
    public bool             isContainer;
    [ShowIf("isContainer")]
    public List<GameItem>   contents;
    public bool             canRead;
    [ShowIf("canRead"), TextArea]
    public List<string> readPages;

    public bool IsThisTheItem(string name)
    {
        string n = name.ToLower();

        if (itemName.ToLower() == n) return true;

        if (synonyms != null)
        {
            foreach (var s in synonyms)
            {
                if (s.ToLower() == n) return true;
            }
        }

        return false;
    }

    public List<GameItem> GetContents(PlayerController player)
    {
        List<GameItem> ret = new List<GameItem>();

        if (isContainer)
        {
            if (contents != null)
            {
                foreach (var item in contents)
                {
                    if (!player.gameState.HasPickedUp(item)) ret.Add(item);
                }
            }
        }

        return ret;
    }
}
