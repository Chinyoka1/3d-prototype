using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Category
{
    public string title;

    public enum ItemType
    {
        Resource,
        Tool,
        Consumable,
        Equippable,
        Armor,
        Weapon,
        HeadArmor,
        BodyArmor,
        LegArmor,
        FeetArmor,
        Ranged,
        Melee,
        Pickaxe,
        Axe,
        Shovel,
        Ammunition,
        LightSource
    }

    public ItemType[] itemTypes;

    public GameObject inventoryContainer;
}