using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Shows an array of tutorial screens on the main menu in an order determined by the screenIndex.
/// 
/// | Authors : Jacques Visser 
/// </summary>

public class TutorialScreens : MonoBehaviour
{
    // A list of Game objects with animations set in the inspector
    [SerializeField] List<GameObject> tutorialScreens;
    [SerializeField] int screenIndex = 0;
    [SerializeField] TMPro.TMP_Text pageNumber;

    public void OnForward()
    {
        if (screenIndex < 3)
            screenIndex++;

        ButtonManager.ButtonSound();
    }

    public void OnBackwards()
    {
        if (screenIndex > 0)
            screenIndex--;

        ButtonManager.ButtonSound();
    }

    public void OnClose()
    {
        GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f).setOnComplete(() => gameObject.SetActive(false));
        ButtonManager.ButtonSound();
    }

    private void Update()
    {
        pageNumber.text = (screenIndex + 1).ToString();

        // For Aku
        // tutorialScreens[screenIndex].SetActive(true)
        for (int i = 0; i < tutorialScreens.Count; i++)
        {
            if (i == screenIndex)
            {
                tutorialScreens[i].SetActive(true);
            }
            else
            {
                tutorialScreens[i].SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        screenIndex = 0;
    }
}
