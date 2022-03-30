using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Data structure used to store aounds in the AudioManager.
/// 
/// | Author: Jacques Visser
/// </summary>

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    //[Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;

    public bool loop;
    public AudioMixerGroup group;

    [HideInInspector]
    public AudioSource source;
}
