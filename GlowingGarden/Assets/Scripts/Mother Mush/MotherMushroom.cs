using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Glowing Garden 2022
/// 
/// Controls dialogue interaction between the player and Mother Mushroom.
/// Responsible for showing / hiding UI dialogue box and UI text prompt.
/// 
/// | Author: Jacques Visser
/// </summary>

public class MotherMushroom : MonoBehaviour
{
    public int dialogueIndex = 0;
    [TextArea(3, 3)]
    [SerializeField] List<string> dialogue;

    [Header("DIALOGUE BOX - DISABLE IN HIERARCHY")]
    public RectTransform dialogueBox;
    public TMP_Text textBox;
    public CanvasGroup prompt;
    public LayerMask player;
    public Vector2 interactionRadius;
    public bool dialogueBoxActive;
    static bool inDialogue;
    public bool playerInRange;

    private void Start()
    {
        dialogueIndex = 0;
        prompt.alpha = 0;
        dialogueBox.transform.localScale = new Vector3(0, 0, 0);
    }

    private void OnMouseDown()
    {
        ManageDialogueBox();
    }

    // Progress the dialogue index
    private void ManageDialogueBox()
    {
        if (!playerInRange) { return; }

        // If the dialogue box is closed, open it.
        if (!dialogueBoxActive) { OpenDialogue(); return; }

        // If the dialogue has ended, close the box.
        if (dialogueIndex == dialogue.Count - 1)
        {
            CloseDialogue();
            return;
        }

        dialogueIndex++;

        // Update the text
        ShowText();
    }

    private void ShowText()
    {
        textBox.text = dialogue[dialogueIndex];
    }

    // Scales the UI window to full-size.
    private void OpenDialogue()
    {
        dialogueBoxActive = true;
        inDialogue = dialogueBoxActive;

        // Lerp dialogue box size and show text when finished lerping.
        dialogueBox.LeanScale(Vector3.one, 0.5f).setOnComplete(ShowText);

        prompt.gameObject.SetActive(false);


        // Invoke the OnInteract event in GameManager.
        GameManager.Interact();
    }

    // Scales the UI window to zero.
    private void CloseDialogue()
    {
        dialogueBoxActive = false;
        inDialogue = dialogueBoxActive;
        dialogueIndex = 0;
        textBox.text = "";

        dialogueBox.LeanScale(Vector3.zero, 0.5f);

        prompt.gameObject.SetActive(true);

        // Invoke the OnDialogueClosed event in GameManager.
        GameManager.CloseBox();
    }

    // Set the text prompt active
    private void OnTriggerEnter2D(Collider2D other)
    {
        LeanTween.cancel(prompt.gameObject);

        prompt.LeanAlpha(1, 0.6f);

        playerInRange = true;
    }

    // Set the text prommpt inactive 
    private void OnTriggerExit2D(Collider2D other)
    {
        LeanTween.cancel(prompt.gameObject);

        prompt.LeanAlpha(0, 0.6f);

        playerInRange = false;

        // In-case the player can actually walk during dialogue for whatever reason, close the dialogue.
        CloseDialogue();
    }

    // Used by the button manager to filter UI Inputs
    public static bool IsDialogueActive()
    {
        return inDialogue;
    }

    // Close Dialogue when escapse is pressed
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && dialogueBoxActive)
        {
            CloseDialogue();
        }
    }
}
