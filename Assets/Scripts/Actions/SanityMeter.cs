using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SanityMeter : MonoBehaviour
{
    public Gradient colorGradient;

    GameState       gameState;
    CanvasGroup     canvasGroup;
    TextMeshProUGUI text;
    
    void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        gameState = player.gameState;

        canvasGroup = GetComponent<CanvasGroup>();

        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        float maxSanity = gameState.GetFloat("max_sanity");
        float sanity = gameState.GetFloat("sanity") / maxSanity;

        if (sanity < 1.0f)
        {
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha + Time.deltaTime);
        }
        else
        {
            canvasGroup.alpha = Mathf.Clamp01(canvasGroup.alpha - Time.deltaTime);
        }

        string tmp = "XXXXXXXXXXXXXXXXXXXXXXX";
        int n = Mathf.FloorToInt(sanity * tmp.Length);

        text.text = tmp.Substring(0, n);
        text.color = colorGradient.Evaluate(sanity);
    }
}
