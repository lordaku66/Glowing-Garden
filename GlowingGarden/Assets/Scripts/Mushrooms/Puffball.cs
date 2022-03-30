using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script controlls the growth of the puffball mushroom.
/// 
/// | Author: Jacques Visser 
/// </summary>

public class Puffball : MonoBehaviour
{
    public Maturity maturity;
    float sizeX;
    float sizeY;
    [Range(0f, 10f)] public float growSpeed = 0;
    const int tickRate = 1000;
    float growRate;
    private Vector2 targetCapSize = Vector2.one;
    [SerializeField] GameObject pin, button, adult;
    GameObject currentCap;

    private Light2D mushroomLight;
    float intensity;
    private DayNightCycle cycle;

    private void Start()
    {
        cycle = FindObjectOfType<DayNightCycle>();

        growRate = growSpeed / tickRate;

        maturity = Maturity.Pin;

        sizeX = pin.transform.localScale.x;
        sizeY = pin.transform.localScale.y;

        AudioManager.Instance.Play("Mushroom Grow");

        currentCap = pin;
    }

    private void Update()
    {
        GrowCap();
        if (maturity == Maturity.Pin)
        {
            currentCap = pin;
            mushroomLight = pin.GetComponent<Light2D>();

            if (pin.transform.localScale.x >= targetCapSize.x - .01f)
            {
                ChangeCap(button);
                maturity = Maturity.Button;
            }
        }

        if (maturity == Maturity.Button)
        {
            currentCap = button;
            mushroomLight = button.GetComponent<Light2D>();

            if (button.transform.localScale.x >= targetCapSize.x - .01f)
            {
                ChangeCap(adult);
                maturity = Maturity.Adult;
            }
        }

        if (maturity == Maturity.Adult)
        {
            currentCap = adult;
            mushroomLight = adult.GetComponent<Light2D>();

            if (adult.transform.localScale.x >= targetCapSize.x - .01f)
            {
                maturity = Maturity.None;
            }
        }

        if (cycle.day)
            mushroomLight.intensity = 0;
        else
        {
            intensity += (1f - mushroomLight.intensity) * .01f;
            mushroomLight.intensity = intensity;
        }
    }

    public void GrowCap()
    {
        // Increase the value of the width & height of the cap's TRANSFORM scale over time.
        sizeX += (targetCapSize.x - currentCap.transform.localScale.x) * growRate;
        sizeY += (targetCapSize.y - currentCap.transform.localScale.y) * growRate;

        // Set the caps's TRANSFORM scale eachframe
        currentCap.transform.localScale = new Vector3(sizeX, sizeY, 1);
    }

    public void ChangeCap(GameObject newCap)
    {
        currentCap.SetActive(false);

        currentCap = newCap;
        sizeX = newCap.transform.localScale.x;
        sizeY = newCap.transform.localScale.y;
        currentCap.SetActive(true);
        AudioManager.Instance.Play("Mushroom Pop");
    }
}
