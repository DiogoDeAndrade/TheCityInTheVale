using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class GameItem : ScriptableObject
{
    public string   itemName;
    public string   onScreenDescription;
    public string[] synonyms;

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
}
