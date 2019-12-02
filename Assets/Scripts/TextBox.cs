using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class TextBox : MonoBehaviour
{
    public float    fadeTime = 0.5f;
    public float    duration = 0.0f;
    public bool     floating = true;
    [ShowIf("floating")]
    public Vector3  floatingOffset;
    [ShowIf("floating")]
    public Camera   gameCamera;

    TextMeshProUGUI  text;
    CanvasGroup      canvasGroup;

    bool            fadeIn;
    float           timeToFade = 0.0f;
    float           fadeTimer;
    Transform       lockedObject;
    RectTransform   rectTransform;
    float           canvasWidth, canvasHeight;
    float           srcWidth, srcHeight;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();

        canvasGroup.alpha = 0.0f;

        CanvasScaler canvasScale = GetComponentInParent<CanvasScaler>();
        canvasWidth = canvasScale.referenceResolution.x;
        canvasHeight = canvasScale.referenceResolution.y;

        if (gameCamera.targetTexture)
        {
            srcWidth = gameCamera.targetTexture.width;
            srcHeight = gameCamera.targetTexture.height;
        }
        else
        {
            srcWidth = Screen.width;
            srcHeight = Screen.height;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((duration > 0) && (timeToFade > 0))
        {
            timeToFade -= Time.deltaTime;
            if (timeToFade < 0)
            {
                canvasGroup.alpha = 1.0f;
                fadeTimer = fadeTime;
                fadeIn = false;
            }
        }

        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime;
            if (fadeTimer <= 0)
            {
                fadeTimer = 0;
                if (fadeIn) canvasGroup.alpha = 1;
                else canvasGroup.alpha = 0;
            }
            else
            {
                if (fadeIn) canvasGroup.alpha = 1 - (fadeTimer / fadeTime);
                else canvasGroup.alpha = fadeTimer / fadeTime;
            }
        }

        UpdatePosition();
    }

    public void SetText(string s, Transform worldObject = null)
    {
        text.text = s;
        timeToFade = duration;
        lockedObject = worldObject;

        if (!fadeIn)
        {
            fadeIn = true;
            fadeTimer = timeToFade;
            canvasGroup.alpha = 0.0f;
        }

        UpdatePosition();
    }

    void UpdatePosition()
    {
        if ((floating) && (lockedObject))
        {
            if (floating)
            {
                Vector3 pos = gameCamera.WorldToScreenPoint(lockedObject.position);

                pos.x = Mathf.Round(pos.x);
                pos.y = Mathf.Round(pos.y);

                pos.x /= srcWidth; pos.y /= srcHeight;

                pos.x *= canvasWidth; pos.y *= canvasHeight;

                rectTransform.anchoredPosition = pos + floatingOffset;
            }
        }
    }
}
