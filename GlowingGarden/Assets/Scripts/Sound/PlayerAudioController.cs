using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    /// <summary>
    /// Glowing Garden 2022
    /// 
    /// Controls Player SFX feedback and when to play it.
    /// 
    /// | Author: Jacques Visser
    /// </summary>

    private AudioManager audioManager;
    private Player player;
    private void OnEnable()
    {
        audioManager = FindObjectOfType<AudioManager>();
        player = FindObjectOfType<Player>();

        GameManager.OnLeftHold += Go;
        GameManager.OnLeftUp += Stop;

        Player.OnJump += Jump;
        Player.OnLongIdle += Charge;

        LandBehavior.OnLand += Land;
    }

    private void Go()
    {
        if (player.grounded)
        {
            if (!audioManager.GetSound("Walk").source.isPlaying)
            {
                audioManager.Play("Walk");
                audioManager.Stop("Drill");
            }
        }
        else
        {
            audioManager.Stop("Walk");
        }

        if (player.digging)
        {
            if (!audioManager.GetSound("Drill").source.isPlaying)
            {
                audioManager.Play("Drill");
                audioManager.Stop("Walk");
            }
        }

        StopCharging();
    }

    private void Stop()
    {
        if (audioManager.GetSound("Walk").source.isPlaying)
        {
            audioManager.Stop("Walk");
        }

        if (audioManager.GetSound("Drill").source.isPlaying)
        {
            audioManager.Stop("Drill");
        }
    }

    private void Jump()
    {
        audioManager.Play("Jump");
        audioManager.GetSound("Jump").source.pitch = UnityEngine.Random.Range(1.2f, 1.6f);

        audioManager.Play("VoiceJump");
        audioManager.GetSound("VoiceJump").source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);

        StopCharging();
    }

    private void Land()
    {
        audioManager.Play("Land");
        audioManager.GetSound("Land").source.pitch = UnityEngine.Random.Range(1f, 1.2f);

        audioManager.Play("VoiceLand");
        audioManager.GetSound("VoiceLand").source.pitch = UnityEngine.Random.Range(0.9f, 1.1f);

        if (!player.onMushroom)
        {
            audioManager.GetSound("Grass").source.panStereo = 0.5f;
            audioManager.Play("Grass");
        }
    }

    private void Charge()
    {
        audioManager.Play("Charging");
    }

    private void StopCharging()
    {
        if (audioManager.GetSound("Charging").source.isPlaying)
        {
            audioManager.Stop("Charging");
        }
    }

    private void OnPause()
    {
        audioManager.GetSound("Walk").source.volume = FindObjectOfType<GameVolume>().SFXVolume();
    }

    private void OnPlay()
    {
        audioManager.GetSound("Walk").source.volume = FindObjectOfType<GameVolume>().SFXVolume();
    }
}
