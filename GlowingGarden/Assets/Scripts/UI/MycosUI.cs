using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls the sprites in the UI slots around the player.
/// Adds myco sprites when collecting and removes them when sprouting mushrooms.
/// 
/// | Author: Krishna Thiruvengadam
/// </summary>

public class MycosUI : MonoBehaviour
{
    public int slotNumber;
    CanvasGroup mycosIcon;
    Transform child;
    Image image;
    bool slotFull = false;
    public int mycosCount = 0;
    Player player;
    public float disappearTime = 0.2f;
    public LeanTweenType easeType;
    const float maxMycos = 6;
    private MushroomSprouter mushroomSprouter;
    private Light2D slotLight;
    private SpawnMycos mycosController;

    void Start()
    {
        player = FindObjectOfType<Player>();
        child = this.gameObject.transform.GetChild(0);
        mycosIcon = child.GetComponent<CanvasGroup>();
        image = mycosIcon.GetComponentInChildren<Image>();

        slotLight = child.GetComponent<Light2D>();

        mushroomSprouter = FindObjectOfType<MushroomSprouter>();
        mycosController = FindObjectOfType<SpawnMycos>();
    }

    private void OnEnable()
    {
        Mycos.MycosEaten += AddMycos;
        Player.OnSprout += ReduceMycos;
        Transitions.UndergroundExit += TurnOffLights;
        Transitions.UndergroundEnter += TurnOnLights;
    }

    void Update()
    {
        if (slotNumber == mycosCount && !slotFull)
        {
            FillSlot();
            slotFull = true;
        }

        if (slotFull)
        {
            if (mycosCount < this.slotNumber)
            {
                child.LeanScale(Vector3.zero, disappearTime).setEase(easeType).setOnComplete(EmptySlot);
                slotFull = false;
            }
        }
    }

    // Add a sprite to the Mycos UI slots.
    void FillSlot()
    {
        mycosIcon.alpha = 1;
        child.localScale = Vector3.one;
        image.sprite = player.GetCollectedMyco().GetComponent<SpriteRenderer>().sprite;

        // Add the mushroom prefab related to this myco to the mushroom sprouter list.
        mushroomSprouter.prefabs.Add(player.GetCollectedMyco().mushroom);

        slotLight.enabled = true;
        slotLight.intensity = 3;
        slotLight.color = mycosController.colors[mycosController.GetSpriteIndex(image.sprite)];
    }

    // Remove sprite from the Mycos UI slots.
    void EmptySlot()
    {
        mycosIcon.GetComponent<Light2D>().enabled = false;
        mycosIcon.LeanAlpha(1, 1f);

        mushroomSprouter.prefabs.RemoveAt(mushroomSprouter.prefabs.Count - 1);

        slotLight.enabled = false;
        slotLight.color = Color.white;
    }

    void TurnOffLights()
    {
        slotLight.enabled = false;
    }

    void TurnOnLights()
    {
        if (slotLight.color != Color.white)
        {
            slotLight.enabled = true;
        }
    }


    // Increment mycos count.
    void AddMycos()
    {
        if (mycosCount >= maxMycos)
            return;
        mycosCount++;


        AudioManager.Instance.Play("Eat");
        AudioManager.Instance.GetSound("Eat").source.pitch = Random.Range(0.9f, 1.4f);
    }

    // Decrement mycos count.
    void ReduceMycos()
    {
        if (mycosCount == 0)
            return;
        mycosCount--;
    }

    // Returns the mycosCount
    public int GetMycos()
    {
        return mycosCount;
    }

    private void OnDisable()
    {
        Mycos.MycosEaten -= AddMycos;
        Player.OnSprout -= ReduceMycos;
        Transitions.UndergroundExit -= TurnOffLights;
        Transitions.UndergroundEnter -= TurnOnLights;
    }
}
