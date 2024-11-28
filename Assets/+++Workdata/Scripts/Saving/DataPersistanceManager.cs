using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Ink.Parsed;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string playerDataFileName;
    [SerializeField] private string worldDataFileName;

    [SerializeField] private bool useEncryption;
    [SerializeField] private GameObject deathScreen;
    private PlayerData _playerData;
    private WorldData _worldData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    
    public static DataPersistanceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Data Persistence Manager in the scene.");
        }

        instance = this;
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, playerDataFileName, worldDataFileName, useEncryption);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        // todo: save scene before loading new scene
        SceneManager.sceneLoaded += LoadSceneObjects;
    }

    public void NewGame()
    {
        _playerData = new PlayerData();
        _worldData = new WorldData();
        
        FindObjectOfType<GameController>().gameMode = GameController.GameMode.NewGame;
        SceneManager.LoadScene("DemoScene");
    }

    public void LoadGame()
    {
        _playerData = dataHandler.LoadPlayerData();
        _worldData = dataHandler.LoadWorldData();
        if (_playerData == null || _worldData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
            return;
        }
        FindObjectOfType<GameController>().gameMode = GameController.GameMode.LoadSaveGame;
        
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadPlayerData(_playerData);
        }
        deathScreen.SetActive(false);
    }

    public void SaveGame()
    {
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref _playerData, ref _worldData);
        }
        
        dataHandler.SavePlayerData(_playerData);
        dataHandler.SaveWorldData(_worldData);
    }

    private void OnApplicationQuit()
    {
        //SaveGame();
    }

    private void LoadSceneObjects(Scene arg0, LoadSceneMode loadSceneMode)
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            // Never load a scene in a LoadWorldData method, otherwise this results in an endless loop
            dataPersistenceObject.LoadWorldData(_worldData);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
