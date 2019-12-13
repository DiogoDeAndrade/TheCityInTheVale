using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressAction : GameAction
{
    public ParticleSystem draftPS;

    readonly List<string> buttons = new List<string>()
    {
        "soth", "gn'thor", "fm'lathgor", "shugg", "uahoth", "ore'e", "loing", "l", "nal"
    };

    int             prevButton = -1;

    protected override void Start()
    {
        base.Start();
    }

    public override string GetVerb()
    {
        return "press";
    }

    public override bool RunAction(PlayerController player, List<string> commandString)
    {
        if (!draftCollider.enabled)
        {
            player.logWindow.AddText("Pressing the buttons seems to do nothing...");
            return true;
        }

        // Check combination
        if (commandString.Count < 2)
        {
            player.logWindow.AddText("What button do you want to press?");
            return true;
        }

        int index = buttons.IndexOf(commandString[1]);

        if (index == -1)
        {
            player.logWindow.AddText("You don't see any button labelled \"" + commandString[1] + "\"");
            return true;
        }

        if (prevButton == -1)
        {
            prevButton = index;
            player.logWindow.AddText("You hear a faint clicking sound as you press " + commandString[1] + "...");
        }
        else
        {
            if (((prevButton == 0) && (index == 8)) ||
                ((prevButton == 8) && (index == 0)))
            {
                player.gameState.SetBool("draft_disabled", true);

                // Success
                player.logWindow.AddText("You hear the grinding of wheels, and the wind in the cave subsides...");
                DisableDraft();
                return true;
            }
            else if (prevButton == index)
            {
                player.logWindow.AddText("That button is already pressed...");
            }
            else
            {
                int nTries = player.gameState.GetInt("draft_tries") + 1;

                switch (nTries)
                {
                    case 1:
                        player.logWindow.AddText("You hear a loud cacophony of sounds in the distance, like metal sliding on stone.");
                        player.logWindow.AddText("The pressed buttons pop up again...");
                        break;
                    case 2:
                        player.logWindow.AddText("You hear the gurgling howl of some horrible creature in the distance.");
                        player.logWindow.AddText("The pressed buttons pop up again...");
                        break;
                    case 3:
                        player.logWindow.AddText("You hear what sounds like the steps of clawed feet far away, but getting closer...");
                        player.logWindow.AddText("Maybe there's some clue for this infernal machine nearby?");
                        player.logWindow.AddText("The pressed buttons pop up again...");
                        break;
                    case 4:
                        player.logWindow.AddText("You don't know what horrors this machine unleashed, but you can almost feel them on top of you...");
                        player.logWindow.AddText("The pressed buttons pop up again...");
                        break;
                    case 5:
                        player.logWindow.AddText("Silence falls around you... Even though you can't hear the creatures anymore, you can sense them nearby, prowling, hunting...");
                        player.logWindow.AddText("The pressed buttons pop up again...");
                        break;
                    case 6:
                        player.logWindow.AddText("");
                        player.logWindow.AddText("You feel the fetid breath of some creature on your neck.");
                        player.logWindow.AddText("As you turn, you feel a sharp pain as claws rake you neck, making you fall to the ground...");
                        player.logWindow.AddText("You feel your blood ebb away, as you become one with the darkness...");
                        player.state = PlayerController.State.Dying;
                        break;
                }

                player.gameState.SetInt("draft_tries", nTries);
                prevButton = -1;
            }
        }

        return true;
    }

    public override void OnLoad(PlayerController player)
    {
        base.OnLoad(player);

        if (player.gameState.GetBool("draft_disabled"))
        {
            DisableDraft();
        }
    }

    void DisableDraft()
    {
        var emission = draftPS.emission;
        emission.enabled = false;

        Collider[] draftColliders = draftPS.GetComponents<Collider>();
        foreach (var draftCollider in draftColliders)
        {
            draftCollider.enabled = false;
        }
    }

}
