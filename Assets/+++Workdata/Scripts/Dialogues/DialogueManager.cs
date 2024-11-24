using System;
using System.Collections.Generic;
using System.Linq;

using Ink;
using Ink.Runtime;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class DialogueManager : MonoBehaviour
{
    private const string SpeakerSeparator = ":";
    private const string EscapedColon = "::";
    private const string EscapedColonPlaceholder = "§";
    private const string PORTRAIT = "portrait";
    private const string LAYOUT = "layout";
    
    public static event Action DialogueClosed;

    ///<summary>Generic Ink event supplying an identifier.</summary>
    public static event Action<string> InkEvent;

    #region Inspector

    [Header("Ink")]

    [SerializeField] private TextAsset inkAsset;

    [Header("UI")]

    [SerializeField] private DialogueBox dialogueBox;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Animator layoutAnimator;

    #endregion

    private Story inkStory;
    private InventoryManager _inventoryManager;
    private ItemManager _itemManager;
    private InkDialogue _inkDialogue;
    
    #region Unity Event Functions

    private void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
        _itemManager = FindObjectOfType<ItemManager>();
        // Initialize Ink.
        inkStory = new Story(inkAsset.text);
        // Add error handling.
        inkStory.onError += OnInkError;
        // Connect an ink function to a C# function.
        inkStory.BindExternalFunction<string>("Event", Event);
        inkStory.BindExternalFunction<string>("Get_Item", Get_Item, true);
        inkStory.BindExternalFunction<string, int>("Add_Item", Add_Item);
    }

    private void OnEnable()
    {
        DialogueBox.DialogueContinued += OnDialogueContinued;
        DialogueBox.ChoiceSelected += OnChoiceSelected;
    }

    private void Start()
    {
        dialogueBox.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        DialogueBox.DialogueContinued -= OnDialogueContinued;
        DialogueBox.ChoiceSelected -= OnChoiceSelected;
    }

    private void OnDestroy()
    {
        inkStory.onError -= OnInkError;
    }

    #endregion

    #region Dialogue Lifecycle

    public void StartDialogue(InkDialogue inkDialogue)
    {
        OpenDialogue();
        _inkDialogue = inkDialogue;
        portraitAnimator.Play("default");
        layoutAnimator.Play("left");

        // Like '-> knot' in ink.
        inkStory.ChoosePathString(inkDialogue.dialoguePath);
        ContinueDialogue();
    }

    private void OpenDialogue()
    {
        dialogueBox.gameObject.SetActive(true);
    }

    private void CloseDialogue()
    {
        EventSystem.current.SetSelectedGameObject(null);
        dialogueBox.gameObject.SetActive(false);
        DialogueClosed?.Invoke();
        _inkDialogue.onDialogueEnded.Invoke();
    }

    private void ContinueDialogue()
    {
        if (IsAtEnd())
        {
            CloseDialogue();
            return;
        }

        DialogueLine line;
        if (CanContinue())
        {
            string inkLine = inkStory.Continue();
            if (string.IsNullOrWhiteSpace(inkLine))
            {
                ContinueDialogue();
                return;
            }
            line = ParseText(inkLine, inkStory.currentTags);
            HandleTags(inkStory.currentTags);
        }
        else
        {
            line = new DialogueLine();
        }

        line.choices = inkStory.currentChoices;

        dialogueBox.DisplayText(line);
    }

    private void OnDialogueContinued(DialogueBox _)
    {
        ContinueDialogue();
    }

    private void OnChoiceSelected(DialogueBox _, int choiceIndex)
    {
        inkStory.ChooseChoiceIndex(choiceIndex);
        ContinueDialogue();
    }

    #endregion

    #region Ink

    private void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be parsed: " + tag);
            }

            string key = splitTag[0].Trim();
            string value = splitTag[1].Trim();

            switch (key)
            {
                case PORTRAIT:
                {
                    portraitAnimator.Play(value);
                    break;
                }
                case LAYOUT:
                {
                    layoutAnimator.Play(value);
                    break;
                }
                default:
                {
                    Debug.LogWarning("Unknown tag key: " + key);
                    break;
                }
            }
        }
    }

    private DialogueLine ParseText(string inkLine, List<string> tags)
    {
        DialogueLine line = new DialogueLine();

        //inkLine = inkLine.Replace(EscapedColon, EscapedColonPlaceholder);

        List<string> parts = inkLine.Split(EscapedColon).ToList();

        string speaker;
        string text;

        switch (parts.Count)
        {
            case 1:
                speaker = null;
                text = parts[0];
                break;
            case 2:
                speaker = parts[0];
                text = parts[1];
                break;
            default:
                Debug.LogWarning($"Ink dialogue line was split at more {SpeakerSeparator} than expected." +
                                 $" Please make sure to use {EscapedColon} for {SpeakerSeparator} inside text");
                goto case 2;
        }

        line.speaker = speaker?.Trim();
        line.text = text.Trim().Replace(EscapedColonPlaceholder, SpeakerSeparator);

        if (tags.Contains("thought"))
        {
            line.text = $"<i>{line.text}</i>";
        }

        return line;
    }

    private bool CanContinue()
    {
        return inkStory.canContinue;
    }

    private bool HasChoices()
    {
        return inkStory.currentChoices.Count > 0;
    }

    private bool IsAtEnd()
    {
        return !CanContinue() && !HasChoices();
    }

    private void OnInkError(string message, ErrorType type)
    {
        switch (type)
        {
            case ErrorType.Author:
                break;
            case ErrorType.Warning:
                Debug.LogWarning(message);
                break;
            case ErrorType.Error:
                Debug.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private void Event(string eventName)
    {
        InkEvent?.Invoke(eventName);
    }

    private object Get_Item(string id)
    {
        Item item = _inventoryManager.FindItemInInventory(id);
        return item != null ? item.amount : 0;
    }

    private void Add_Item(string id, int amount)
    {
        int durability = _itemManager.GetItemInfo(id).maxDurability;
        _itemManager.Add(new Item(id, amount, durability));
    }

    #endregion
}

public struct DialogueLine
{
    public string speaker;
    public string text;
    public List<Choice> choices;

    // Here we can also add other information like speaker images or sounds.
    //public Sprite speakerImage;
}
