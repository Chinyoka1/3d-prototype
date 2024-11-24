using UnityEngine;
using System;
using UnityEngine.Events;

public class InkDialogue : MonoBehaviour
{
    #region Inspector

    [Tooltip("Path to a specified knot.stitch in the ink file.")]
    public string dialoguePath;

    public bool startOnAwake;
    
    public UnityEvent onDialogueEnded;
    
    #endregion

    private void Awake()
    {
        if (startOnAwake)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if (string.IsNullOrWhiteSpace(dialoguePath))
        {
            Debug.LogWarning("No dialogue path defined", this);
            return;
        }
        
        FindObjectOfType<GameController>().StartDialogue(this);
    }
}