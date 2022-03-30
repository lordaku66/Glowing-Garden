using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script sets the color alpha of the star and cloud backgrounds during the transitions
/// of the day / night cycle.
/// 
/// | Author: Jacques Visser
/// </summary>

public class BackgroundFader : MonoBehaviour
{
    [Header("CLOUD & STAR BACKGROUNDS")]

    [SerializeField] List<SpriteRenderer> clouds = new List<SpriteRenderer>();
    [SerializeField] List<SpriteRenderer> stars = new List<SpriteRenderer>();
    public float opacity;
    public float inverseOpacity;
    DayNightCycle dayNight;

    private void Start()
    {
        dayNight = FindObjectOfType<DayNightCycle>();
    }

    private void Update()
    {
        if (DayNightCycle.SkyAlpha != 0)
        {
            opacity = DayNightCycle.SkyAlpha;
            inverseOpacity = 1 - opacity;

            if (dayNight.hour >= dayNight.sunset - 3 && dayNight.hour <= dayNight.sunset + 3)
            {
                foreach (SpriteRenderer s in stars)
                {
                    s.color = new Color(s.color.r, s.color.g, s.color.b, opacity);
                }
                foreach (SpriteRenderer s in clouds)
                {
                    s.color = new Color(s.color.r, s.color.g, s.color.b, inverseOpacity);
                }
            }
            else if (dayNight.hour >= dayNight.sunrise - 3 && dayNight.hour <= dayNight.sunrise + 3)
            {
                foreach (SpriteRenderer s in stars)
                {
                    s.color = new Color(s.color.r, s.color.g, s.color.b, inverseOpacity);
                }
                foreach (SpriteRenderer s in clouds)
                {
                    s.color = new Color(s.color.r, s.color.g, s.color.b, opacity);
                }
            }
        }
    }
}
