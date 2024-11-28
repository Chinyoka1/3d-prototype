using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemChecker : MonoBehaviour
{
    public string[] items;
    [SerializeField] private UnityEvent checkPositive;
    [SerializeField] private UnityEvent checkNegative;

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void CheckForItems()
    {
        bool hasItemsInInventory = true;
        foreach (string id in items)
        {
            if (!_inventoryManager.CheckForItem(id))
            {
                hasItemsInInventory = false;
            }
        }

        if (hasItemsInInventory)
        {
            checkPositive.Invoke();
        }
        else
        {
            checkNegative.Invoke();
        }
    }
}
