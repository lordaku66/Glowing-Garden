using UnityEngine;

/// <summary>
/// Glowing Gaarden 2022
/// 
/// This script destroys an 'X' collision particles when its animations are complete.
/// 
/// | Author: Jacques Visser
/// </summary>

public class CollisionParticle : MonoBehaviour
{
    private Animator anim;
    public float delay;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
