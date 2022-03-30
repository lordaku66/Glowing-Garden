using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script controls the state of the digging light attached to the player. The light 
/// is turned on when the player enters the underground and is turned off when resurfacing.
/// 
/// | Author: Jacques Visser
/// </summary>

public class DiggingLight : MonoBehaviour
{
    private Light2D digLight;
    [SerializeField] private float maxIntensity = 0;

    private void OnEnable()
    {
        digLight = GetComponent<Light2D>();
        Transitions.UndergroundEnter += OnGroundEnter;
        Transitions.UndergroundExit += OnGroundExit;
    }

    public void OnGroundEnter()
    {
        digLight.intensity = maxIntensity;
    }

    public void OnGroundExit()
    {
        digLight.intensity = 0;
    }

    private void OnDisable()
    {
        Transitions.UndergroundEnter -= OnGroundEnter;
        Transitions.UndergroundExit -= OnGroundExit;
    }
}
