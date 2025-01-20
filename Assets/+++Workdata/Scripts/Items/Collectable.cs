using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private UnityEvent onCollected;
    
    [Header("Regrowing Collectable")]
    
    [SerializeField] private bool regrow;
    [SerializeField] private float regrowTime = 20;
    [SerializeField] private GameObject visualRoot;
    [SerializeField] private Interactable interactable;

    private bool growing;
    private float growingTime;

    private void FixedUpdate()
    {
        if (growing)
        {
            growingTime += Time.deltaTime;
            float growingPercentage = growingTime / regrowTime;
            visualRoot.transform.localScale = new Vector3(growingPercentage, growingPercentage, growingPercentage);
        }
    }

    public void Collect()
    {
        onCollected.Invoke();
        FindObjectOfType<ItemManager>().Add(item);

        if (!regrow)
        {
            Destroy(gameObject);
        }
        else
        {
            visualRoot.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            interactable.Deselect();
            interactable.enabled = false;
            
            StartCoroutine(Regrow());
        }
    }

    IEnumerator Regrow()
    {
        growing = true;
        growingTime = 0;
        
        yield return new WaitForSeconds(regrowTime);
        
        visualRoot.transform.localScale = new Vector3(1, 1, 1);
        
        interactable.enabled = true;
        growing = false;
    }
}
