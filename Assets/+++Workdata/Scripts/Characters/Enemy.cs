using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Camera mainCam;
    private GameObject worldSpaceCanvas;
    private void Awake()
    {
        mainCam = Camera.main;
        worldSpaceCanvas = GetComponentInChildren<Canvas>().gameObject;
    }

    private void Update()
    {
        worldSpaceCanvas.transform.rotation = mainCam.transform.rotation;
    }
}
