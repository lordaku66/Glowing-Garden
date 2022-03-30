using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This script instantiates a maximum number of mycos and respawns eaten nutrients
/// one minute after being consumed by the player.
/// The color, size and positions of the nutrients are randomized.
///  
/// | Author: Krishna Thiruvengadam
/// </summary>

public class SpawnMycos : MonoBehaviour
{
    public GameObject mycosPrefab;
    public int mycosLimit = 10;
    public LayerMask whereIsSpawn;
    Vector3 spawnLoc;
    Collider2D circle;

    float respawnTimer = 0f;
    int respawnCount = 0;

    public Sprite[] sprites;

    [SerializeField] List<GameObject> mushroomPrefabs = new List<GameObject>();
    string[] spriteLights = { "#c6d831", "#488bd4", "#ed4c40", "#686f99", "#ff417d", "#dbd8f6", "#7a09fa", "#47f641", "#ffc825", "#0cf1ff", "#3dff6e", "#ecab11", "#d4662f", "#4fa4b8", "#ff5277", "#8b97b6" };

    public List<Color> colors = new List<Color>();
    Color newColor;
    int spriteIndex;
    Light2D spriteLight;

    private void OnEnable()
    {
        Mycos.MycosEaten += CheckRespawnCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        circle = mycosPrefab.GetComponent<CircleCollider2D>();
        spawnLoc.z = -0.5f;

        // Spawn nutrients initially
        for (int i = 0; i < mycosLimit; i++)
        {
            SpawnNutrient();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check how many times to respawn a nutrient
        if (respawnCount > 0)
        {
            respawnTimer += Time.deltaTime;
            // Respawn a nutrient after every minute
            if (respawnTimer > 60f)
            {
                SpawnNutrient();
                respawnCount--;
                respawnTimer = 0f;
            }
        }
    }

    void CheckRespawnCount()
    {
        respawnCount++;
    }

    void SpawnNutrient()
    {
        // Random spawn location with const z value
        spawnLoc.x = Random.Range(-18, 18f);
        spawnLoc.y = Random.Range(-35f, -1f);

        float scaleFactor = Random.Range(1f, 1.6f);

        // Prevent spawning on top of another
        circle = Physics2D.OverlapCircle(spawnLoc, 2f, whereIsSpawn);

        if (circle == false)
        {
            var newMyco = Instantiate(mycosPrefab, spawnLoc, Quaternion.identity, transform);
            newMyco.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1);

            // Random sprite
            spriteIndex = UnityEngine.Random.Range(0, sprites.Length - 1);

            newMyco.GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
            newMyco.GetComponent<Mycos>().mushroom = mushroomPrefabs[spriteIndex];

            // Set the light
            spriteLight = newMyco.GetComponent<Light2D>();
            spriteLight.color = colors[spriteIndex];
        }
    }

    public int GetSpriteIndex(Sprite sprite)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprite == sprites[i])
            {
                return i;
            }
        }
        return 0;
    }
}
