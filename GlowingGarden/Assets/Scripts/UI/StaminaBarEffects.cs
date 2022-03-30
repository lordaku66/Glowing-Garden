using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controlls any additional effects and SFX that play as result of the stamina meter being drained or filled. 
/// 
/// | Author: Jacques Visser 
/// </summary>

public class StaminaBarEffects : MonoBehaviour
{
    private Stamina energy;
    public CanvasGroup border;

    private void OnEnable()
    {
        energy = FindObjectOfType<Stamina>();
        Stamina.OnMeterEmpty += PingPong;
        Stamina.OnMeterFilled += Show;

        // Stamina.OnMeter75 += On75;
        // Stamina.OnMeter50 += On50;
        // Stamina.OnMeter25 += On25;
        // Stamina.OnMeter1 += On1;
    }

    public void PingPong()
    {
        LeanTween.cancel(border.gameObject);
        border.LeanAlpha(0, 0.5f).setLoopPingPong();
        AudioManager.Instance.Play("Refill4");
        AudioManager.Instance.Play("Empty");
    }

    private void Show()
    {
        AudioManager.Instance.Play("Refill5");
        LeanTween.cancel(border.gameObject);

        LeanTween.cancel(border.gameObject);
        border.LeanAlpha(1, 0.5f);
    }

    // private void On75()
    // {
    //     AudioManager.Instance.Play("Refill1");
    // }

    // private void On50()
    // {
    //     AudioManager.Instance.Play("Refill2");
    // }

    // private void On25()
    // {
    //     AudioManager.Instance.Play("Refill3");
    // }

    // private void On1()
    // {
    //     AudioManager.Instance.Play("Refill4");
    // }
}
