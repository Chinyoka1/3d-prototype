using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Collectables : MonoBehaviour
{
    [SerializeField] private List<Item> items;
    [SerializeField] private UnityEvent onCollected;

    public void Collect()
    {
        onCollected.Invoke();
        FindObjectOfType<ItemManager>().Add(items);
        Destroy(gameObject);
    }
}
