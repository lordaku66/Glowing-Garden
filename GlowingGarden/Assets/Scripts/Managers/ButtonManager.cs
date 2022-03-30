using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Opens and closes menu UI depending on the current game state.
/// Methods below are called using  OnClick callbacks assigned on the respectibe button components
/// in the inspector.
///  
/// | Author: Krishna Thiruvengadaem
/// </summary>

public class ButtonManager : MonoBehaviour
{
    [Header("Start Menu Fields")]
    public CanvasGroup mainMenuCG;
    private GameManager gameManager;
    public GameObject startMenu;

    [Header("Pause Menu Fields")]
    public GameObject pauseMenu;

    [Header("Message Panels")]
    public GameObject settingsWindow;
    public GameObject quitWindow;
    public GameObject tutorials;
    public GameObject creditsWindow;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        GameManager.OnPlay += Resume;
        GameManager.OnPause += Pause;
    }

    void Resume()
    {
        startMenu.SetActive(false);
        pauseMenu.SetActive(false);

        HideWindow();
    }

    void Pause()
    {
        // Check the player is not it dialogue to not bring up the pause menu;
        if (MotherMushroom.IsDialogueActive()) { return; }

        if (!gameManager.mainMenu)
        {
            pauseMenu.SetActive(true);
        }
    }

    void CloseMenu()
    {
        gameManager.mainMenu = false;
        gameManager.mainMenu = false;
    }

    public void Quit()
    {
        ButtonSound();
        Application.Quit();
    }

    public void CreditsButton()
    {
        creditsWindow.SetActive(true);
        creditsWindow.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);
        ButtonSound();
    }

    public void CloseCredits()
    {
        creditsWindow.GetComponent<CanvasGroup>().LeanAlpha(0, 0.5f).setOnComplete(() => creditsWindow.SetActive(false));
        ButtonSound();
    }

    public void HelpButton()
    {
        tutorials.SetActive(true);
        tutorials.GetComponent<CanvasGroup>().LeanAlpha(1, 0.5f);
        ButtonSound();
    }

    // Attached to the play button on the main menu.
    public void PlayButton()
    {
        mainMenuCG.LeanAlpha(0, 0.5f).setOnComplete(CloseMenu);
        GameManager.Play();
        AudioManager.Instance.Play("MenuPlay");
    }

    // Attched to the pause button in-game.
    public void PauseButton()
    {
        if (gameManager.paused)
        {
            GameManager.Play();
        }
        else { GameManager.Pause(); }
    }

    // Shows the settings window either on the main menu or in-game.
    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
        settingsWindow.LeanScale(Vector3.one, 0.5f).setEase(LeanTweenType.easeSpring);
        ButtonSound();
        AudioManager.Instance.Play("MenuOpen");
    }

    // Shows the quit window either on the main menu or in-game.
    public void QuitButton()
    {
        quitWindow.SetActive(true);
        quitWindow.LeanScale(Vector3.one, 0.5f).setEase(LeanTweenType.easeSpring);
        ButtonSound();
        AudioManager.Instance.Play("MenuOpen");
    }

    public void HideWindow()
    {
        ButtonSound();
        if (settingsWindow.activeSelf)
        {
            settingsWindow.LeanScale(Vector3.zero, 0.5f).setEase(LeanTweenType.easeSpring).setOnComplete(() => settingsWindow.SetActive(false));
            AudioManager.Instance.Play("MenuClose");
        }

        if (quitWindow.activeSelf)
        {
            quitWindow.LeanScale(Vector3.zero, 0.5f).setEase(LeanTweenType.easeSpring).setOnComplete(() => quitWindow.SetActive(false));
            AudioManager.Instance.Play("MenuClose");
        }
    }

    public static void ButtonSound()
    {
        AudioManager.Instance.Play("MenuButton");
    }
}
