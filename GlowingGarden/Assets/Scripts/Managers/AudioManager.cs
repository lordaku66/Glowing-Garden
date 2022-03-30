using System;
using UnityEngine.Audio;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Singleton Audio Manager Instance. Accessed Statically with AudioManager.Instance
/// Audio is stored as a Sound object.
/// AudioManager.Play("") / AudioManager.Stop("")
///
/// | Author: Jacques Visser
/// </summary>

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }
    public AudioMixerGroup Music;
    public AudioMixerGroup SFX;
    public AudioMixerGroup Ambience;

    private GameVolume volume;
    private float currentFreq;
    public AudioMixer ambienceMixer;

    private void Awake()
    {
        volume = FindObjectOfType<GameVolume>();
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); return; }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;


            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = s.group;
            s.source.loop = s.loop;

            if (s.group == SFX)
            {
                s.source.volume = volume.SFXVolume();
            }
            if (s.group == Music)
            {
                s.source.volume = volume.MusicVolume();
            }
        }
    }

    private void Update()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.outputAudioMixerGroup == Music)
            {
                if (s.source.volume != volume.MusicVolume())
                {
                    s.source.volume = volume.MusicVolume();
                }
            }

            if (s.source.outputAudioMixerGroup == SFX || s.source.outputAudioMixerGroup == Ambience)
            {
                if (s.source.volume != volume.SFXVolume())
                {
                    s.source.volume = volume.SFXVolume();
                }
            }
        }
    }

    private void Start()
    {
        Play("Day");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null) { Debug.LogError("'" + name + "'" + " is not found in the list of audio clip names."); return; }

        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public Sound GetSound(string name)
    {
        return Array.Find(sounds, sound => sound.name == name);
    }

    public float GetLowPassFreq()
    {
        bool result = ambienceMixer.GetFloat("lowpassFreq", out currentFreq);

        if (result)
            return currentFreq;
        else
            return 0f;
    }
}
