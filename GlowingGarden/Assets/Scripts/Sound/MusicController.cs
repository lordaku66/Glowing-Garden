using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls the Music playing in the background depending on if the player is overground
/// or underground.
/// 
/// | Author: Jacques Visser
/// </summary>

public class MusicController : MonoBehaviour
{
    private DayNightCycle cycle;


    private void OnEnable()
    {
        cycle = FindObjectOfType<DayNightCycle>();

        Transitions.UndergroundEnter += Underground;
        Transitions.UndergroundExit += Overground;
    }

    private void Start()
    {
        AudioManager.Instance.Play("Overground Music");
    }

    public void Underground()
    {
        AudioManager.Instance.Stop("Overground Music");
        AudioManager.Instance.Play("Underground Music");
    }

    public void Overground()
    {
        AudioManager.Instance.Stop("Underground Music");

        if (cycle.day)
            AudioManager.Instance.Play("Overground Music");
        else
            return;
    }
}
