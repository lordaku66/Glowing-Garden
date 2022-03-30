using System;
using UnityEngine;

/// <summary>
/// Glowing Garden 2022
/// 
/// Singleton Game Manager. Accessed publicly with GameManager.Instance
/// Registers some delegate events that are used to control:
/// 
/// 1. Player Input & Player Animation States
/// 2. Gamestate e.g. paused, playing, main menu
/// 
/// Other scripts subscribe to the events in OnEnable to trigger output using events.
/// 
/// | Author: Jacques Visser
/// </summary>

public enum GameStates
{
    MainMenu,
    Gameplay,
    Paused
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    public static event Action OnLeftMouse;
    public static event Action OnLeftHold;
    public static event Action OnLeftUp;

    public static event Action OnRightMouse;
    public static event Action OnRightHold;
    public static event Action OnRightUp;

    public static event Action OnPause;
    public static event Action OnPlay;

    public static event Action OnInteract;
    public static event Action OnDialogueClose;
    public bool mainMenu;
    public bool paused;

    private MycosUI mycos;
    public static bool hasMycos;

    private void Start()
    {
        instance = this;

        mycos = FindObjectOfType<MycosUI>();

        if (mainMenu)
        {
            Pause();
        }
    }

    private void Update()
    {
        hasMycos = mycos.mycosCount > 0;

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftMouse?.Invoke();
        }
        if (Input.GetMouseButton(0))
        {
            OnLeftHold?.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnLeftUp?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouse?.Invoke();
        }
        if (Input.GetMouseButton(1))
        {
            OnRightHold?.Invoke();
        }
        if (Input.GetMouseButtonUp(1))
        {
            OnRightUp?.Invoke();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (mainMenu) { return; }

            if (!paused) { Pause(); }
            else if (!paused && MotherMushroom.IsDialogueActive()) { Debug.Log("Wait for dialogue to close"); }

            else if (paused) { Play(); }

        }
    }
    public static void Pause()
    {
        if (MotherMushroom.IsDialogueActive()) { return; }

        Debug.Log("Paused");
        Instance.paused = true;
        OnPause?.Invoke();
    }

    public static void Play()
    {
        Debug.Log("Play");
        Instance.paused = false;
        OnPlay?.Invoke();
    }

    public static void Interact()
    {
        OnInteract?.Invoke();
    }

    public static void CloseBox()
    {
        OnDialogueClose?.Invoke();
    }
}
