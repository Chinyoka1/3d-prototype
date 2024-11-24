using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private PlayerController playerController;
    private DialogueManager dialogueManager;

    public enum GameMode
    {
        MainMenu,
        NewGame,
        LoadSaveGame,
        LoadScene,
        DialogueMode,
        DebugMode
    }

    public GameMode gameMode;
    public Button lastSelectable;
    #region Unity Event Functions

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnEnable()
    {
        DialogueManager.DialogueClosed += EndDialogue;
    }

    private void Start()
    {
        if(playerController)
            EnterPlayMode();
    }

    private void OnDisable()
    {
        DialogueManager.DialogueClosed -= EndDialogue;
    }

    #endregion

    #region Modes

    public void EnterPlayMode()
    {
        Time.timeScale = 1;
        // In the editor: Unlock with ESC.
        //Cursor.lockState = CursorLockMode.Locked;
        playerController.EnableInput();
    }

    private void EnterDialogueMode()
    {
        Time.timeScale = 1;
        //Cursor.lockState = CursorLockMode.Locked;
        playerController.DisableInput();
    }

    #endregion

    public void StartDialogue(InkDialogue inkDialogue)
    {
        dialogueManager.StartDialogue(inkDialogue);
        EnterDialogueMode();
    }

    private void EndDialogue()
    {
        EnterPlayMode();
    }
    
    public void EnterPauseMode()
    {
        Time.timeScale = 0;
        playerController.DisableInput();
    }

    public void SetLastSelectable()
    {
        SetSelectable(lastSelectable);
    }
    public void SetSelectable(Button newSelactable)
    {
        Selectable newSelectable;
        lastSelectable = newSelactable;
        newSelectable = newSelactable;

        //newSelactable.Select();
        StartCoroutine(DelayNewSelectable(newSelectable));
    }

    public void ExitMenu()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    IEnumerator DelayNewSelectable(Selectable newSelectable)
    {
        yield return null;
        newSelectable.Select();
    }
}

