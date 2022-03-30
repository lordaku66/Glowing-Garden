using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script controlls the growth of all mushrooms with a stipe and cap.
/// A mushroom can be a pin, a button or an adult.
/// The growth rate and target dimentions of each level of maturity can be set in the inspector.
/// 
/// | Author: Jacques Visser 
/// </summary>

[System.Serializable]
public enum Maturity
{
    None,
    Pin,
    Button,
    Adult
}

public class Mushrooms : MonoBehaviour
{
    [Header("MUSHROOM AGE")]
    public Maturity maturity = Maturity.None;
    private GameObject cap;
    private GameObject stipe;
    private SpriteRenderer capSprite;
    private SpriteRenderer stipeSprite;
    private Vector3 startScale;
    private float capOrigin = 0;
    Vector3 targetCapSize = Vector3.one;

    [Header("MAX STIPE HEIGHTS ")]
    public Vector2 buttonStipeScale;
    public Vector2 adultStipeScale;
    private Vector2 targetStipeScale;

    [Header("CAP GROWTH SPEED |  STIPE GROWTH SPEED")]
    [Range(0, 10)] public float capSpeed = 1;
    [Range(0, 10)] public float stipeSpeed = 1;
    private int tickRate = 1000;
    float capGrowthRate;
    float stipeGrowthRate;
    float capX = 0f;
    float capY = 0f;
    float stipeY = 0f;
    float stipeX = 0f;
    float stipeOrigin;

    [Header("CAP OFFSET FINE-TUNING")]
    [Range(-1f, 1f)] public float capOffset = 0f;
    [SerializeField] float buttonOffset;
    [SerializeField] float adultOffset;

    [Header("CAP OBJECTS")]
    [SerializeField] private GameObject pinCap;
    [SerializeField] private GameObject buttonCap;
    [SerializeField] private GameObject adultCap;

    [Header("STIPE OBJECTS")]
    [SerializeField] private GameObject smallStipe;
    [SerializeField] private GameObject bigStipe;
    DayNightCycle cycle;

    private void OnEnable()
    {
        AudioManager.Instance.Play("Mushroom Grow");
    }
    private void Start()
    {
        // Set refences to starting sprites
        cap = pinCap;
        stipe = smallStipe;

        // Get cap starting scale
        startScale = cap.transform.localScale;
        capX = startScale.x;
        capY = startScale.y;

        // Get cap starting position 
        capOrigin = cap.transform.localPosition.y;

        // Set the rate at which the caps and stipes grow
        capGrowthRate = capSpeed / tickRate;
        stipeGrowthRate = stipeSpeed / tickRate;

        // Get the stipe's sprite width and height
        stipeSprite = stipe.GetComponent<SpriteRenderer>();
        stipeY = stipeSprite.size.y;
        stipeX = stipeSprite.size.x;

        // Get origin height of the stipe before growing
        stipeOrigin = stipe.transform.localPosition.y;

        cycle = FindObjectOfType<DayNightCycle>();

        // Start as pin size.
        SetState(Maturity.Pin);
    }

    public void SetState(Maturity newMaturity)
    {
        //Debug.Log(name + " has reached the the maturity of " + newMaturity.ToString());
        maturity = newMaturity;
        AudioManager.Instance.Play("Mushroom Pop");
    }

    private void Update()
    {
        if (maturity == Maturity.Pin)
        {
            if (pinCap.transform.localScale.x >= targetCapSize.x - .01f)
            {
                SetCap(buttonCap);
                SetState(Maturity.Button);
            }
        }
        else
        {
            // Grow the mushrooms stipe so long as it is not in the pin state.
            GrowStipe();
        }

        if (maturity == Maturity.Button)
        {
            targetStipeScale = buttonStipeScale;

            if (buttonCap.transform.localScale.x >= targetCapSize.x - .01f)
            {
                SetCap(adultCap);
                SetStipe(bigStipe);
                SetState(Maturity.Adult);
            }
        }

        if (maturity == Maturity.Adult)
        {
            targetStipeScale = adultStipeScale;

            if (adultCap.transform.localScale.x >= targetCapSize.x - .01f)
            {
                SetState(Maturity.None);
            }
        }

        if (maturity == Maturity.None)
        {
            if (adultCap.transform.localScale.x >= targetCapSize.x - .01f)
            {
                adultCap.transform.localScale = Vector3.one;
                return;
            }
        }

        // Grow the cap each frame.
        GrowCap();
    }


    // Changes the cap sprite
    public void SetCap(GameObject newCap)
    {
        cap.SetActive(false);

        cap = newCap;
        capY = newCap.transform.localScale.y;
        capX = newCap.transform.localScale.x;
        cap.transform.position = Vector3.zero;

        capSprite = newCap.GetComponent<SpriteRenderer>();
        cap.SetActive(true);
    }

    // Chages stipe sprite
    public void SetStipe(GameObject newStipe)
    {
        stipe.SetActive(false);

        stipe = newStipe;
        stipeSprite = newStipe.GetComponent<SpriteRenderer>();
        stipeOrigin = newStipe.transform.localPosition.y;

        stipeY = stipeSprite.size.y;
        stipeX = stipeSprite.size.x;

        stipe.SetActive(true);
    }

    // Scale the cap each frame to reach the tager cap size set in the inspector. 
    public void GrowCap()
    {
        // Increase the value of the width & height of the cap's TRANSFORM scale over time.
        capX += (targetCapSize.x - cap.transform.localScale.x) * capGrowthRate;
        capY += (targetCapSize.y - cap.transform.localScale.y) * capGrowthRate;

        // Set the caps's TRANSFORM scale eachframe
        cap.transform.localScale = new Vector3(capX, capY, 1);
    }


    // Grow the stipe and move the cap upwards proportianlly to vertical growth.
    public void GrowStipe()
    {
        float targetOffset = 0;

        if (maturity == Maturity.Button)
            targetOffset = buttonOffset;
        else
            targetOffset = adultOffset;

        // Increase the height value of the stipe's SPRITE over time.
        stipeY += (targetStipeScale.y - stipeSprite.size.y) * stipeGrowthRate;
        stipeX += (targetStipeScale.x - stipeSprite.size.x) * stipeGrowthRate;

        // Set the SPRITES new size.
        stipeSprite.size = new Vector2(stipeX, stipeY);

        var yPos = (capOffset + capSprite.bounds.size.y / 2);

        // Update Stipe & Cap position & height
        cap.transform.localPosition = new Vector3(stipe.transform.localPosition.x, stipe.transform.localPosition.y + yPos, 0);

        capOffset += (targetOffset - capOffset) * stipeGrowthRate;
    }
}