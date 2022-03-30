using System;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls the transition between the overground and underground.
/// Burrow() and Resurface() moves the player disables any player input whilst transitioning.
/// 
/// | Author: Jacques Visser
/// </summary>

public class Transitions : MonoBehaviour
{
    public static event Action UndergroundExit;
    public static event Action UndergroundEnter;
    public static event Action OnReady;

    public static void Burrow(GameObject player)
    {
        Debug.Log("Burrow");

        LeanTween.cancel(player);

        // Move player down.
        player.LeanMoveLocalY(0.85f, 0.5f).setOnComplete((OnReady));

        // Disable player gravity.
        Player.Instance.GetBody().gravityScale = 0;
        Player.Instance.GetBody().velocity = Vector2.zero;

        // Make them go upside dowm.
        Player.Instance.playerSprite.transform.LeanRotateZ(180, 0.25f).setEase(LeanTweenType.easeSpring);
        AudioManager.Instance.Play("Entering Underground");

        // Play burrow SFX.
        UndergroundEnter?.Invoke();
    }

    public static void Resurface(GameObject player)
    {
        Debug.Log("Resurface");

        LeanTween.cancel(player);

        // Move Player up and Rotate to 0.
        player.LeanMoveLocalY(1.43f, 0.5f).setOnComplete((OnReady));
        Player.Instance.playerSprite.LeanRotateZ(0, 0.25f).setEase(LeanTweenType.easeSpring);

        // Enable player gravity.
        Player.Instance.GetBody().gravityScale = 1;

        // Play Resuface SFX.
        AudioManager.Instance.Play("Exiting Underground");

        UndergroundExit.Invoke();
    }
}
