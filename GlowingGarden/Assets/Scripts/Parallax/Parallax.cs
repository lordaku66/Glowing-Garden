using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Creates a parallax effect between 4 different background layers that are children of the camera.
/// Tiles backgrounds infinitely horizontally.
/// 
/// Created using the help of this YouTube Tutorial by "Dani"
/// https://www.youtube.com/watch?v=zit45k6CUMk
///  
/// | Author: Jacques Visser
/// </summary>

public class Parallax : MonoBehaviour
{
    private float length;
    private float startpos;
    public GameObject cam;
    [SerializeField] private float parallaxAmmount = 0;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        var temp = (cam.transform.position.x * (1 - parallaxAmmount));
        var distance = (cam.transform.position.x * parallaxAmmount);

        transform.position = new Vector3(startpos + distance, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
