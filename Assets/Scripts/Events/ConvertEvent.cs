using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertEvent : GameEvent
{
    public GameItem item;
    public GameItem convertTo;
    public List<string> conversionMessage;


    protected override void RunEvent(PlayerController player)
    {
        if (player.gameState.HasItem(item))
        {
            player.gameState.RemoveFromInventory(item);
            player.gameState.AddItemToInventory(convertTo);

            player.Read(conversionMessage);
        }
    }
}
