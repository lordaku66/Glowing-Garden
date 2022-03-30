using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Sets the Ambient SFX being played depending on the time of day.
/// 
/// | Author: Jacques Visser
/// </summary>

public class DayNightAudioController : MonoBehaviour
{
    private AudioManager audioManager;
    private DayNightCycle cycle;
    public AudioMixerGroup group;

    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
        cycle = FindObjectOfType<DayNightCycle>();

        DayNightCycle.OnDay += Day;
        DayNightCycle.OnNight += Night;
    }

    private void Day()
    {
        audioManager.Play("Day");

        if (audioManager.GetSound("Night").source.isPlaying)
            audioManager.Stop("Night");
    }

    private void Night()
    {
        audioManager.Play("Night");

        if (audioManager.GetSound("Day").source.isPlaying)
            audioManager.Stop("Day");

        // if (audioManager.GetSound("Overground Music").source.isPlaying)
        //     audioManager.Stop("Overground Music");
    }
}
