using System.Collections;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls the cutoff frequency of a the low-pass filter for sounds in the 'Ambience'
/// mixer channel. 
/// 
/// Reduces the cutoff frequncy while the player is underground to make bird song and cricket noises
/// soound distant.
/// 
/// | Author: Jacques Visser
/// </summary>

public class LowPassFilter : MonoBehaviour
{
    public float frequency;
    private float maxFreq = 5000;
    private float minFreq = 100;

    private void OnEnable()
    {
        Transitions.UndergroundEnter += StartFilter;
        Transitions.UndergroundExit += StopFilter;
    }

    private void Start()
    {
        frequency = AudioManager.Instance.GetLowPassFreq();
    }

    private void StartFilter()
    {
        StopAllCoroutines();
        StartCoroutine(Deafen());
    }

    private void StopFilter()
    {
        StopAllCoroutines();
        StartCoroutine(Undeafen());
    }

    // Reduce the cut-off frequency to the minimum
    private IEnumerator Deafen()
    {

        if (AudioManager.Instance.GetLowPassFreq() == minFreq)
        {
            frequency = minFreq;
            yield return 0;
        }

        while (frequency > minFreq)
        {
            frequency += (minFreq - AudioManager.Instance.GetLowPassFreq()) * .05f;
            AudioManager.Instance.ambienceMixer.SetFloat("lowpassFreq", frequency);
            yield return null;
        }
        frequency = maxFreq;
    }

    // Increase the cut-off frequency to the default.
    private IEnumerator Undeafen()
    {
        frequency = AudioManager.Instance.GetLowPassFreq();
        if (AudioManager.Instance.GetLowPassFreq() == maxFreq)
        {
            yield return 0;
        }

        while (frequency < maxFreq)
        {
            frequency += (maxFreq - AudioManager.Instance.GetLowPassFreq()) * .05f;
            AudioManager.Instance.ambienceMixer.SetFloat("lowpassFreq", frequency);
            yield return null;
        }
        frequency = minFreq;
    }
}
