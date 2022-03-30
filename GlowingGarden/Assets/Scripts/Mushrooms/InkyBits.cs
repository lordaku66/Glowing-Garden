using UnityEngine;

/// <summary>
/// Glowing Garden 2022  
/// 
/// Grows the drippy bits hanging off Coprinus Comatus' Mushroom cap at a constant rate.
/// 
/// | Author: Jacques Visser
/// </summary>

public class InkyBits : MonoBehaviour
{
    float sizeY;
    public float targetSize;
    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.size = new Vector2(spriteRenderer.size.x, 0);
        sizeY = spriteRenderer.size.y;
    }

    private void Update()
    {
        if (GetComponent<SpriteRenderer>().size.y < targetSize)
        {
            sizeY += (targetSize - spriteRenderer.size.y) * .002f;
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, sizeY);
        }
    }
}
