using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteracted;
    [SerializeField] private UnityEvent onSelected;
    [SerializeField] private UnityEvent onDeselected;
    public UnityEvent onDisabled;
    public UnityEvent onEnabled;

    private void Start()
    {
        List<Interaction> interactions = GetComponentsInChildren<Interaction>(true).ToList();

        if (interactions.Count > 0)
        {
            interactions[0].gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        onDisabled.Invoke();
    }

    private void OnEnable()
    {
        onEnabled.Invoke();
    }

    public void Interact()
    {
        if (!enabled) return;
        Interaction interaction = FindActiveInteraction();

        if (interaction != null)
        {
            interaction.Execute();
        }
        
        onInteracted.Invoke();
    }

    public void Select()
    {
        if (!enabled) return;
        onSelected.Invoke();
    }

    public void Deselect()
    {
        onDeselected.Invoke();
    }

    private Interaction FindActiveInteraction()
    {
        return GetComponentInChildren<Interaction>(false);
    }
}
