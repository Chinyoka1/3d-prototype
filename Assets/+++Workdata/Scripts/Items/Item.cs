using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public string id;
    public int amount;
    public int durability;

    public Item(string id, int amount, int durability = 0)
    {
        this.id = id;
        this.amount = amount;
        this.durability = durability;
    }
}
