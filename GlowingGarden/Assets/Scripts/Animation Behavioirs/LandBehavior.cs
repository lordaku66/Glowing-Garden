using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This animation behavior invokes a deleagte event when the player animator exits the landing state.
/// The OnLand event is used to play the correct audio depending on the durface the player lands on.
/// 
/// | Author: Jacques Visser 
/// </summary>

public class LandBehavior : StateMachineBehaviour
{
    public static event Action OnLand;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        OnLand?.Invoke();
    }
}
