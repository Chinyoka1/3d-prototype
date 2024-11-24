using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class PlayerData
{
    // PlayerDamageable
    public int health;
    // PlayerMovement
    public float playerPosX;
    public float playerPosY;
    public string scene;
    // InventorySlots
    public SerializeableDictionary<string, Item> inventorySlots;
    // EquipmentSlots
    public SerializeableDictionary<string, Item> equipmentSlots;
    // StoryProgressSaver
    public SerializeableDictionary<string, int> sceneStoryProgresses;
    // QuestManager
    public List<string> openQuests;
    public List<string> finishedQuests;
    public string activeQuestId;
    // HairstyleManager
    public int currentHairstyleId;
    
    public PlayerData()
    {
        health = 100;
        inventorySlots = new SerializeableDictionary<string, Item>();
        equipmentSlots = new SerializeableDictionary<string, Item>();
        sceneStoryProgresses = new SerializeableDictionary<string, int>();
    }
}
