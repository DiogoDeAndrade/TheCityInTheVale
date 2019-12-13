using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTextEvent : GameEvent
{
    [TextArea]
    public List<string> text;

    protected override void RunEvent(PlayerController player)
    {
        player.Read(text);
    }
}
