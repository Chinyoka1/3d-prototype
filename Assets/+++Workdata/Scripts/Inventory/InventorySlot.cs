using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class InventorySlot : MonoBehaviour, IDataPersistence
{
    [SerializeField]private string id;

    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = Guid.NewGuid().ToString();
    }

    public bool saveAndLoad = true;
    
    public bool canShowItemDisplay = true;
    public bool hasTakeButton;
    public UnityEvent<Item> onItemTaken;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private Toggle slotToggle;
    [SerializeField] private ItemInfo _itemInfo;
    [SerializeField] private int _amount;
    [SerializeField] private int _durability;
    [SerializeField] private Button _takeButton;
    [SerializeField] private Slider durabilitySlider;
    private ItemDisplay _itemDisplay;
    private ItemManager _itemManager;

    private void Awake()
    {
        _itemDisplay = FindObjectOfType<ItemDisplay>(true);
        _itemManager = FindObjectOfType<ItemManager>();
    }

    private void OnEnable()
    {
        slotToggle.onValueChanged.AddListener(delegate
        {
            HandleSlotToggle();
        });

        if (hasTakeButton && _takeButton != null)
        {
            _takeButton.onClick.AddListener(TakeItemToInventory);
        }
    }

    private void OnDisable()
    {
        slotToggle.onValueChanged.RemoveListener(delegate
        {
            HandleSlotToggle();
        });

        if (hasTakeButton && _takeButton != null)
        {
            _takeButton.onClick.RemoveListener(TakeItemToInventory);
        }
    }

    private void SetSlot()
    {
        itemImage.sprite = _itemInfo.icon;
        itemImage.gameObject.SetActive(true);
        if (itemAmount)
        {
            itemAmount.text = _amount.ToString();
            itemAmount.gameObject.SetActive(true);
        }
        slotToggle.interactable = true;
        durabilitySlider.gameObject.SetActive(_itemInfo.maxDurability > 0);
        durabilitySlider.maxValue = _itemInfo.maxDurability;
        durabilitySlider.value = _durability;
    }

    private void HandleSlotToggle()
    {
        if (canShowItemDisplay && slotToggle.isOn && _itemInfo.id != null)
        {
            _itemDisplay.SetItemInfo(_itemInfo, _durability);
        }
        else
        {
            _itemDisplay.gameObject.SetActive(false);
        }

        if (hasTakeButton && slotToggle.isOn && _itemInfo.id != null && _takeButton != null)
        {
            _takeButton.gameObject.SetActive(true);
        }
        else if (_takeButton != null)
        {
            _takeButton.gameObject.SetActive(false);
        }
    }

    private void TakeItemToInventory()
    {
        _itemManager.Add(GetItem());
        onItemTaken.Invoke(GetItem());
        StartCoroutine(WaitThenClear());
    }

    IEnumerator WaitThenClear()
    {
        yield return null;
        Clear();
    }

    public void SetItemInfo(ItemInfo itemInfo, int amount, int durability)
    {
        _itemInfo = itemInfo;
        _amount = amount;
        _durability = durability;
        SetSlot();
    }

    public void SetItemAmount(int amount)
    {
        _amount = amount;
        SetSlot();
    }

    public void SetItemDurability(int durability)
    {
        _durability = durability;
        durabilitySlider.value = _durability;
    }

    public void Clear()
    {
        _itemInfo = null;
        _amount = 0;
        itemImage?.gameObject.SetActive(false);
        itemAmount?.gameObject.SetActive(false);
        slotToggle.interactable = false;
        slotToggle.isOn = false;
        durabilitySlider.gameObject.SetActive(false);
    }

    public Item GetItem()
    {
        if (_itemInfo != null)
        {
            return new Item(_itemInfo.id, _amount, _durability);
        }

        return null;
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        if (saveAndLoad)
        {
            if (playerData.inventorySlots.TryGetValue(id, out Item item))
            {
                if (item == null || item.id == null)
                {
                    Clear();
                }
                else
                {
                    _itemManager = FindObjectOfType<ItemManager>();
                    ItemInfo itemInfo = _itemManager.GetItemInfo(item.id);
                    if (itemInfo == null || itemInfo.id == null)
                    {
                        Clear();
                    }
                    else
                    {
                        SetItemInfo(itemInfo, item.amount, item.durability);
                    }
                }
            }
        }
    }

    public void LoadWorldData(WorldData worldData)
    {
    }

    public void SaveData(ref PlayerData playerData, ref WorldData worldData)
    {
        if (saveAndLoad)
        {
            if (playerData.inventorySlots.ContainsKey(id))
            {
                playerData.inventorySlots[id] = GetItem();
            }
            else if (id != null)
            {
                playerData.inventorySlots.Add(id, GetItem());
            }
        }
    }
}
