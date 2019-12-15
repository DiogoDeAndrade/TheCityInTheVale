using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineAndDie : ExamineAction
{
    public AudioClip scratchingSound;

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
        SoundManager.PlaySound(SoundManager.SoundType.SoundFX, scratchingSound, 0.5f, 1.0f);

        while (player.state == PlayerController.State.ModalText) yield return null;

        yield return new WaitForSeconds(10.0f);

        SoundManager.PlaySound(SoundManager.SoundType.SoundFX, scratchingSound, 0.75f, 0.9f);

        player.Read(new List<string>() { "The scratching sound grows louder..." });

        while (player.state == PlayerController.State.ModalText) yield return null;

        yield return new WaitForSeconds(5.0f);

        SoundManager.PlaySound(SoundManager.SoundType.SoundFX, scratchingSound, 1.0f, 0.8f);

        player.Read(new List<string>() { "You look around desperately, trying to locate the source of\nthe noise...\nYou sense impending doom coming for you..." });

        while (player.state == PlayerController.State.ModalText) yield return null;

        yield return new WaitForSeconds(5.0f);

        player.Read(new List<string>() { "You feel a sharp pain in your back...\nYou try to look back, but another burst of pain knocks you down...\nYou scream, feeling that your horror has only just begun...\n\nYou should have never searched for this accursed city..." });

        player.state = PlayerController.State.Dying;
    }
}
