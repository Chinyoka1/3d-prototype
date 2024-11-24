using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    private ItemInfo _itemInfo;
    private ItemManager _itemManager;
    private Damageable playerDamageable;
    private int itemDurability;

    private void Awake()
    {
        _itemManager = FindObjectOfType<ItemManager>();
        playerDamageable = FindObjectOfType<PlayerController>().damageable;
    }

    public void SetItemInfo(ItemInfo itemInfo, int durability)
    {
        _itemInfo = itemInfo;
        itemDurability = durability;
        image.sprite = itemInfo.icon;
        nameText.text = itemInfo.name;
        descriptionText.text = itemInfo.description;
        gameObject.SetActive(true);
    }
}