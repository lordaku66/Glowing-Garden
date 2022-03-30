using System.Collections;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script lerps the intensity of any attached light2d component over time.
/// The intensity lights are lerped towards is controlled by the day / night cycle.
/// 
/// | Author: Jacques Visser
/// </summary>

public class NightLight : MonoBehaviour
{
    public float dur = 3;
    public float start = 0;
    public float end = 1.2f;
    private float valToLerp;
    public bool ignoreLerp;

    private Light2D light2D;
    private Player player;
    private DayNightCycle cycle;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        cycle = FindObjectOfType<DayNightCycle>();
        light2D = GetComponent<Light2D>();

        // Turn the light off on start
        valToLerp = start;

        // Set the intensity value on start
        if (cycle.day)
        {
            light2D.intensity = 0;
        }
        else
        {
            light2D.intensity = end;
        }

        if (cycle.night)
        {
            ignoreLerp = true;
        }
    }

    private void OnEnable()
    {
        DayNightCycle.OnDay += OnLightOff;
        DayNightCycle.OnNight += OnLightOn;
    }

    private void Update()
    {
        if (!ignoreLerp)
        {
            light2D.intensity = valToLerp;
        }

        if (cycle.day && ignoreLerp) { ignoreLerp = false; }
    }

    // On night - fade light intensity to maximum
    public void OnLightOn()
    {
        if (light2D.intensity != end)
        {
            StartCoroutine(FadeIn());
        }
    }

    // On day - fade light intensity to 0
    public void OnLightOff()
    {
        if (light2D.intensity != start)
        {
            StartCoroutine(FadeOut());
        }
    }

    IEnumerator FadeIn()
    {
        if (light2D.intensity == end)
        {
            yield return 0;
        }

        float timeElapsed = 0;
        while (timeElapsed < dur)
        {
            valToLerp = Mathf.Lerp(start, end, timeElapsed / dur);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valToLerp = end;
    }

    IEnumerator FadeOut()
    {
        if (light2D.intensity == start)
        {
            yield return 0;
        }

        float timeElapsed = 0;
        while (timeElapsed < dur)
        {
            valToLerp = Mathf.Lerp(end, start, timeElapsed / dur);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valToLerp = start;
    }

    private void OnDisable()
    {
        light2D.intensity = 0;

        DayNightCycle.OnDay -= OnLightOff;
        DayNightCycle.OnNight -= OnLightOn;
    }

}
