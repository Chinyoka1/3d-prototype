using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private UnityEvent onCollected;

    public void Collect()
    {
        onCollected.Invoke();
        FindObjectOfType<ItemManager>().Add(item);
        Destroy(gameObject);
    }
}
