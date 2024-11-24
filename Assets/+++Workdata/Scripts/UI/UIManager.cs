using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuContainer;
    private GameController gameController;
    private PlayerController playerController;
    private InventoryManager inventoryManager;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    private void OnEnable()
    {
        playerController.inputActions.UI.Pause.performed += TogglePauseMenu;
        playerController.inputActions.UI.Inventory.performed += inventoryManager.ToggleInventory;
    }

    private void OnDisable()
    {
        playerController.inputActions.UI.Pause.performed -= TogglePauseMenu;
        playerController.inputActions.UI.Inventory.performed -= inventoryManager.ToggleInventory;
    }

    private void TogglePauseMenu(InputAction.CallbackContext context)
    {
        pauseMenuContainer.SetActive(!pauseMenuContainer.activeSelf);
        if (pauseMenuContainer.activeSelf)
        {
            gameController.EnterPauseMode();
        }
        else
        {
            gameController.EnterPlayMode();
        }
    }
    
    public void SaveGame()
    {
        DataPersistanceManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        DataPersistanceManager.instance.LoadGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

}