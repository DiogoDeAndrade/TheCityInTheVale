using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEvent : GameEvent
{
    [TextArea]
    public string deathText;

    protected override void RunEvent(PlayerController player)
    {
        player.logWindow.AddText(deathText);
        player.state = PlayerController.State.Dying;
    }
}
