using System;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Uses Unitys OnMouseOver and Circle Collider to check if the player's mouse is at a range
/// more than a the threshold which begins to cause gimbal issues for the player.
/// 
/// | Author: Jacques Visser 
/// </summary>

public class MouseRangeCheck : MonoBehaviour
{
    public static event Action OnMouseValid;
    public static event Action OnMouseInvalid;
    public static bool tooClose;

    private void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnPlay += OnPlay;
    }

    private void OnMouseOver()
    {
        if (!tooClose) { OnMouseInvalid?.Invoke(); tooClose = true; }
    }

    private void OnMouseExit()
    {
        OnMouseValid?.Invoke();
        tooClose = false;
    }
    private void OnPause() { gameObject.SetActive(false); }
    private void OnPlay() { gameObject.SetActive(true); }

    private void Update()
    {
        transform.position = Player.Instance.transform.position;
    }
}
