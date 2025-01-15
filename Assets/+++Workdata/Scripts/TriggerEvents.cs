using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider> onEnter;
    [SerializeField] private UnityEvent<Collider> onExit;
    
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            onEnter.Invoke(col);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            onExit.Invoke(col);
        }
    }
}
