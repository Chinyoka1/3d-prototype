using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInfo
{
    public string id;
    public string name;
    public string description;
    public Sprite icon;
    public int maxStackSize;
    public Category.ItemType[] types;
    
    // consumeable items
    public int healingPoints;
    public int damagePoints;
    
    // armor
    public float armorPercentage;
    
    // equipable items
    public int maxDurability;

    // weapons
    public string attackAnimationTrigger;
    
    // light sources
    public float lightIntensity;
    
    // ammunition
    public GameObject projectilePrefab;
}
