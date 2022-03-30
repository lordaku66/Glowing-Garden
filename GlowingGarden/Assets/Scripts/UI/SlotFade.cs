using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls the opacity of the Mycos slots around the player.
/// 
/// | Author: Jacques Visser
/// </summary>

public class SlotFade : MonoBehaviour
{
    private CanvasGroup cg;
    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;
    private void OnEnable()
    {
        cg = GetComponent<CanvasGroup>();

        Transitions.UndergroundEnter += Show;
        Transitions.UndergroundExit += Hide;
    }

    void Show()
    {
        LeanTween.cancel(cg.gameObject);
        cg.LeanAlpha(1, fadeInTime).setEase(LeanTweenType.easeInOutSine);
    }

    void Hide()
    {
        LeanTween.cancel(cg.gameObject);
        cg.LeanAlpha(0, fadeOutTime).setEase(LeanTweenType.easeInOutSine).setDelay(2);
    }

    private void OnDisable()
    {
        Transitions.UndergroundEnter -= Show;
        Transitions.UndergroundExit -= Hide;
    }
}