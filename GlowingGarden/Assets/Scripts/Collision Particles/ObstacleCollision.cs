using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// This scrips instantiates an 'X' collision particle when then player is colliding with an
/// underground obstacle.
/// 
/// | Author: Jacques Visser
/// </summary>

public class ObstacleCollision : MonoBehaviour
{
    private Player player;

    [SerializeField] float spawnTimer;
    const float spawnRate = 0.8f;
    public GameObject X;
    public Transform drill;
    private void Start()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (player.stuck)
        {
            // Spawn 'x' Every so often;
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnRate)
            {
                Instantiate(X, drill.position, Quaternion.identity);
                spawnTimer = 0;
            }
        }
        else
        {
            spawnTimer = spawnRate;
        }
    }
}
