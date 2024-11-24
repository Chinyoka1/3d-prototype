using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemInfo[] itemInfos;
    public GameObject popupPrefab;
    public Transform itemPopupContainer;

    private InventoryManager _inventoryManager;

    private void Awake()
    {
        _inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void Add(Item item, bool showPopup = true)
    {
        AddToInventory(item.id, item.amount, item.durability, showPopup);
        /*if (_inventoryManager.activeInventoryPart.inventorySlots.Length > _inventoryManager.activeInventoryPart.filledSlots)
        {
            AddToInventory(item.id, item.amount);
        }
        else
        {
            // todo
            print("inventory is full");
        }*/
    }

    public void Add(List<Item> items)
    {
        foreach (Item item in items) Add(item);
    }

    public ItemInfo GetItemInfo(string id)
    {
        foreach (ItemInfo itemInfo in itemInfos)
        {
            if (itemInfo.id == id) return itemInfo;
        }

        return null;
    }

    private void AddToInventory(string id, int amount, int durability, bool showPopup)
    {
        Item item = _inventoryManager.FindItemInInventory(id);

        if (item == null)
        {
            Item newItem = new Item(id, amount, durability);
            _inventoryManager.AddNewItem(newItem);
        }
        else
        {
            _inventoryManager.IncreaseItemAmount(id, amount);
        }

        if (showPopup)
        {
            StartCoroutine(CreateItemPopup(id, amount));
        }
    }

    public void RemoveItem(string id, int amount)
    {
        _inventoryManager.RemoveItem(id, amount);
        StartCoroutine(CreateItemPopup(id, -amount));
    }

    IEnumerator CreateItemPopup(string id, int amount)
    {
        ItemPopup itemPopup = Instantiate(popupPrefab, itemPopupContainer).GetComponent<ItemPopup>();
        itemPopup.SetItem(new Item(id, amount));
        yield return new WaitForSeconds(2);
        Destroy(itemPopup.gameObject);
    }
}
