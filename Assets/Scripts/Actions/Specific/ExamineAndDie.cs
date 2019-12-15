using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineAndDie : ExamineAction
{
    Coroutine dieInABitCR;

    protected override bool RunAction(PlayerController player, List<string> commandString)
    {
        if (base.RunAction(player, commandString))
        {
            if (dieInABitCR == null) dieInABitCR = StartCoroutine(DieInABit(player));
            return true;
        }

        return false;
    }

    IEnumerator DieInABit(PlayerController player)
    {
        yield return new WaitForSeconds(10.0f);

        player.Read(new List<string>() { "The scratching sound grows louder..." });

        yield return new WaitForSeconds(5.0f);

        player.Read(new List<string>() { "You look around desperately, trying to locate the source of\nthe noise...\nYou sense impending doom coming for you..." });

        yield return new WaitForSeconds(5.0f);

        player.Read(new List<string>() { "You feel a sharp pain in your back...\nYou try to look back, but another burst of pain knocks you down...\nYou scream, feeling that your horror has only just begun...\n\nYou should have never searched for this accursed city..." });

        player.state = PlayerController.State.Dying;
    }
}
