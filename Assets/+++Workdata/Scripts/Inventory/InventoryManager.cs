using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public UnityEvent onItemsChanged;
    [SerializeField] private GameObject inventoryContainer;
    [SerializeField] private GameObject itemDisplay;
    public InventorySlot[] inventorySlots;
    public int filledSlots;

    private ItemManager _itemManager;

    private void Awake()
    {
        _itemManager = FindObjectOfType<ItemManager>();
        InitialClear();
    }

    private void InitialClear()
    {
        _itemManager = FindObjectOfType<ItemManager>();
        foreach (InventorySlot inventorySlot in inventorySlots) inventorySlot.Clear();
    }

    public void ToggleInventory(InputAction.CallbackContext context)
    {
        inventoryContainer.SetActive(!inventoryContainer.activeSelf);
        itemDisplay.SetActive(false);
        if (inventoryContainer.activeSelf)
        {
            FindObjectOfType<GameController>().EnterPauseMode();
        }
        else
        {
            FindObjectOfType<GameController>().EnterPlayMode();
        }
    }

    public Item FindItemInInventory(string id)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            Item item = inventorySlot.GetItem();
            if (item != null && item.id == id)
            {
                return item;
            }
        }

        return null;
    }

    public void AddNewItem(Item item)
    {
        ItemInfo itemInfo = _itemManager.GetItemInfo(item.id);
        if (itemInfo == null) return;

        if (item.amount > itemInfo.maxStackSize)
        {
            // add item stacks to inventory until the amount is smaller than the max stack size
            int rest = item.amount - itemInfo.maxStackSize;
            for (int i = 0; rest > 0; i++)
            {
                AddItem(new Item(item.id, itemInfo.maxStackSize, item.durability));
                if (item.amount > itemInfo.maxStackSize)
                {
                    rest = item.amount - itemInfo.maxStackSize;
                }
                else
                {
                    rest = 0;
                }

                item.amount = rest;
            }
        }
        else
        {
            AddItem(item);
        }

        onItemsChanged.Invoke();
    }

    private void AddItem(Item item)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            if (inventorySlot.GetItem() == null || inventorySlot.GetItem().id == null)
            {
                ItemInfo itemInfo = _itemManager.GetItemInfo(item.id);
                inventorySlot.SetItemInfo(itemInfo, item.amount, item.durability);
                filledSlots++;
                return;
            }
        }
    }

    public void IncreaseItemAmount(string id, int amount)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            Item item = inventorySlot.GetItem();
            if (item != null)
            {
                if (item.id == id)
                {
                    ItemInfo itemInfo = _itemManager.GetItemInfo(item.id);
                    int newAmount = item.amount + amount;
                    if (newAmount <= itemInfo.maxStackSize)
                    {
                        inventorySlot.SetItemInfo(itemInfo, newAmount, item.durability);
                    }
                    else
                    {
                        int amountToFill = itemInfo.maxStackSize - item.amount;
                        int newSlotAmount = amount - amountToFill;
                        inventorySlot.SetItemInfo(itemInfo, itemInfo.maxStackSize, item.durability);
                        AddNewItem(new Item(id, newSlotAmount, item.durability));
                    }
                }
            }
        }

        onItemsChanged.Invoke();
    }

    public void RemoveItem(string id, int amount)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            Item itemInSlot = inventorySlot.GetItem();
            if (itemInSlot != null && itemInSlot.id == id)
            {
                if (itemInSlot.amount <= amount)
                {
                    inventorySlot.Clear();
                    filledSlots++;
                }
                else
                {
                    inventorySlot.SetItemAmount(itemInSlot.amount - amount);
                }
                return;
            }
        }

        onItemsChanged.Invoke();
    }

    public void TurnOfffUI()
    {
        inventoryContainer.SetActive(false);
        itemDisplay.SetActive(false);
    }
}