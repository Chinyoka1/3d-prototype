using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image iconImage;
    private int _amount;
    private ItemInfo _itemInfo;


    public void SetItem(Item item)
    {
        _amount = item.amount;
        _itemInfo = FindObjectOfType<ItemManager>().GetItemInfo(item.id);
        SetItemPopup();
    }

    private void SetItemPopup()
    {
        itemText.text = (_amount > 0 ? "+" : "") + _amount + " " + _itemInfo.name;
        iconImage.sprite = _itemInfo.icon;
    }
}
