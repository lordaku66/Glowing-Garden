using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Instantiates mushroom prefabs stored in a list of GameObjects at the soil level when the
/// player resurfaces.
/// 
/// | Author: Jacques Visser
/// </summary>

public class MushroomSprouter : MonoBehaviour
{
    public List<GameObject> prefabs = new List<GameObject>();
    public Transform parentTransform;
    private Vector3 spawnPos;
    public float GroundY;

    private void OnEnable()
    {
        Player.OnSprout += SpawnMushroom;
    }

    public void SpawnMushroom()
    {
        spawnPos = new Vector3(Player.GetX(), GroundY, 0);
        var newMushroom = Instantiate(prefabs[prefabs.Count - 1], spawnPos, Quaternion.identity, parentTransform);
    }

    private void OnDisable()
    {
        Player.OnSprout -= SpawnMushroom;
    }
}
