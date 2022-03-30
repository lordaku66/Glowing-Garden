using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Sets the volume of the Music & SFX mixer channels depending on the value of
/// UI sliders in the games settings.
/// 
/// | Author: Jacques Visser
/// </summary>

[ExecuteInEditMode]
public class GameVolume : MonoBehaviour
{
    [Header("MUSIC & SFX VOLUME - SET BY UI VOLUME SLIDERS")]
    [SerializeField][Range(0, 1f)] private float music;
    [SerializeField][Range(0, 1f)] private float sfx;
    public TMPro.TMP_Text musicVal;
    public TMPro.TMP_Text sfxVal;
    public AudioManager am;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Update()
    {
        sfx = sfxSlider.value;
        music = musicSlider.value;

        sfxVal.text = (sfxSlider.value * 100).ToString("0") + "%";
        musicVal.text = (musicSlider.value * 100).ToString("0") + "%";
    }

    public float MusicVolume()
    {
        return music;
    }

    public float SFXVolume()
    {
        return sfx;
    }
}
