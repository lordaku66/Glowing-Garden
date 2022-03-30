using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script controls the length of the day and night in the game.
/// It uses 24 hour time and a custon accessible tick rate to control the color of the sky and the 
/// intesnity of the overground lights. 
/// 
/// | Author: Krishna Thiruvengadam
/// </summary>

public class DayNightCycle : MonoBehaviour
{
    public static event Action OnDay;
    public static event Action OnNight;

    [Header("CYCLE POSITION & SPEED")]

    [Tooltip("Speed of the day/night cycle")]
    public float tick;
    private float seconds;
    [SerializeField] private int mins;
    [Tooltip("24 Hour Time")]
    public int hour;
    private int days = 1;

    [Header("DAY & NIGHT SWITCH HOURS")]
    public int sunrise = 7;
    public int sunset = 21;

    [Header("ACTIVATE SCENE LIGHTS")]
    public bool activateLights;
    public Color dayTime;
    public Color nightTime;

    [Header("DEBUG STATES")]
    public bool day;
    public bool night;
    Camera mainCamera;

    int k = 24000;

    void Start()
    {
        day = true;
        mainCamera = Camera.main;
        mainCamera.backgroundColor = dayTime;
    }

    void FixedUpdate()
    {
        CalcTime();
    }

    public void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 60)
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60)
        {
            mins = 0;
            hour += 1;
        }

        if (hour >= 24)
        {
            hour = 0;
            days += 1;
        }

        ControlSkyColor();
    }

    // Check the time of day and update light intensity & sky color at sunrise & sunset
    public void ControlSkyColor()
    {
        // Set time to fade to be inversely proportional to the tick rate
        float time = k / tick;

        if (hour >= sunset - 3 && hour < sunset + 3)
        {
            StartCoroutine(LerpColor(dayTime, nightTime, time));

            if (activateLights == false) // if lights havent been turned on
            {
                if (hour >= sunset + 1 && mins == 30)
                {
                    OnNight?.Invoke();

                    night = true;
                    day = false;
                    activateLights = true;
                }
            }
        }

        if (hour >= sunrise - 3 && hour < sunrise + 3)
        {
            StartCoroutine(LerpColor(nightTime, dayTime, time));

            if (activateLights == true) // if lights are on
            {
                if (hour >= sunrise)
                {
                    OnDay?.Invoke();

                    night = false;
                    day = true;
                    activateLights = false;
                }
            }
        }
    }

    // Statically accessed alpha value of the sky backgrounds relative to the color of the sky.
    public static float SkyAlpha { get; set; }

    // Lerp the color of the sky over a perioid of 6 in-game hours
    IEnumerator LerpColor(Color a, Color b, float time)
    {
        float inversedTime = 1 / time;
        for (float step = 0.0f; step < 1.0f; step += Time.deltaTime * inversedTime)
        {
            mainCamera.backgroundColor = Color.Lerp(a, b, step);
            SkyAlpha = step;

            yield return null;
        }
    }
}
