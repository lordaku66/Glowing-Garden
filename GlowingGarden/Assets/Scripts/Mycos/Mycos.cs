using System;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script desroys a nutrient when the player enters the nutrients collider.
/// 
/// | Author: Krishna Thiruvengadam
/// </summary>

public class Mycos : MonoBehaviour
{
    public static event Action MycosEaten;
    private MycosUI mycosUI;
    public GameObject mushroom;
    DayNightCycle cycle;
    bool bright = false;
    public bool highlighted;
    [SerializeField] float highlightRadius = 0.5f;
    public SpriteRenderer highlightSprite;
    public LayerMask whatIsPlayer;


    // Start is called before the first frame update
    void Start()
    {
        mycosUI = FindObjectOfType<MycosUI>();
        cycle = GameObject.FindObjectOfType<DayNightCycle>();
    }

    // Update is called once per frame
    void Update()
    {
        highlighted = Physics2D.OverlapCircle(transform.position, highlightRadius, whatIsPlayer);
        highlightSprite.enabled = highlighted;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (mycosUI.GetMycos() == 6)
                return;

            MycosEaten?.Invoke();
            LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInElastic).setOnComplete(DestroyMycos);
        }
    }

    void DestroyMycos()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, highlightRadius);
    }
}
