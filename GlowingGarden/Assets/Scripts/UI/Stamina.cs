using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls the players stamina meter draining and re-filling.
/// 
/// | Author: Jacques Visser
/// </summary>

public class Stamina : MonoBehaviour
{

    public static event Action OnMeterFilled;
    public static event Action OnMeter75;
    public static event Action OnMeter50;
    public static event Action OnMeter25;
    public static event Action OnMeter1;
    public static event Action OnMeterEmpty;

    private Player player;
    private CanvasGroup cg;
    public float fadeTime;
    public static Stamina instance;
    public float solarEnergy = 1;

    [Header("Energy Meter Drain Rate - 0 - 0.01")]
    [SerializeField] private float drainRate = 0.002f;
    public Image meter;
    public bool drained;

    public bool q1, q2, q3, q4;

    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        GameManager.OnLeftHold += OnLeft;
        Transitions.UndergroundEnter += ShowMeter;
        Transitions.UndergroundExit += OnExit;

        OnMeterFilled += HideMeter;

        cg = GetComponent<CanvasGroup>();
    }

    // Fade meter in.
    public void ShowMeter()
    {
        LeanTween.cancel(cg.gameObject);
        cg.LeanAlpha(1, fadeTime);
        OnExit();
    }

    // Fade meter out.
    public void HideMeter()
    {
        LeanTween.cancel(cg.gameObject);
        cg.LeanAlpha(0, fadeTime);
    }

    private void Update()
    {
        solarEnergy = meter.fillAmount;

        // Drain stamina meter 
        if (!player.digging)
        {
            if (solarEnergy < 1.0f)
            {
                meter.fillAmount += 0.2f * drainRate;
                AudioManager.Instance.Stop("Empty");

                if (!q4 && drained)
                {
                    //OnMeter1?.Invoke();
                    q4 = true;
                }
            }

            if (solarEnergy == 1 && cg.alpha == 1)
            {
                OnMeterFilled?.Invoke();
                drained = false;
            }
        }

        if (player.digging && !drained)
        {
            if (solarEnergy == 0)
            {
                OnMeterEmpty?.Invoke();
                drained = true;
                AudioManager.Instance.Play("Empty");
            }
        }

        if (solarEnergy >= 0.749f && solarEnergy < 0.751f)
        {
            if (!q1)
            {
                //OnMeter75?.Invoke();
                q1 = true;
            }
        }
        if (solarEnergy >= 0.499f && solarEnergy < 0.501f)
        {
            if (!q2)
            {
                //OnMeter50?.Invoke();
                q2 = true;
            }
        }
        if (solarEnergy >= 0.249 && solarEnergy < 0.251f)
        {
            if (!q3)
            {
                //OnMeter25?.Invoke();
                q3 = true;
            }
        }
    }

    private void OnLeft()
    {
        // Check the player is underground and moving.
        if (player.digging && !player.stuck)
        {
            if (solarEnergy > 0f)
            {
                meter.fillAmount -= 0.1f * drainRate;
            }
        }
    }

    private void OnExit()
    {
        q1 = false;
        q2 = false;
        q3 = false;
        q4 = false;
        drained = false;
    }

    private void OnDisable()
    {
        GameManager.OnLeftMouse -= OnLeft;
        Transitions.UndergroundEnter -= ShowMeter;
        Transitions.UndergroundExit -= HideMeter;
    }
}
