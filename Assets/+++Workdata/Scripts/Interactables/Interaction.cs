using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interaction : MonoBehaviour
{
    [SerializeField] private UnityEvent onInteracted;
    [SerializeField] private Interaction nextInteraction;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        List<Interaction> interactions = transform.parent.GetComponentsInChildren<Interaction>().ToList();

        foreach (Interaction interaction in interactions)
        {
            // Skip self
            if (interaction == this) continue;
            
            interaction.gameObject.SetActive(false);
        }
    }

    public void Execute()
    {
        if (nextInteraction != null)
        {
            nextInteraction.gameObject.SetActive(true);
        }
        
        onInteracted.Invoke();
    }
}
