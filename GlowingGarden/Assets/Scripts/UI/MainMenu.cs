using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Sets main menu active depending on the state of the 'mainMenu' bool in GameManager.
/// 
/// | Author: Jacques Visser
/// </summary>

public class MainMenu : MonoBehaviour
{
    private GameManager gm;

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();

        gameObject.SetActive(gm.mainMenu);
    }
}
