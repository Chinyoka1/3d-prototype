using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string playerDataFileName;
    private string worldDataFileName;
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "OMORI";

    public FileDataHandler(string dataDirPath, string playerDataFileName, string worldDataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.playerDataFileName = playerDataFileName;
        this.worldDataFileName = worldDataFileName;
        this.useEncryption = useEncryption;
    }

    public PlayerData LoadPlayerData()
    {
        string fullPath = Path.Combine(dataDirPath, playerDataFileName);
        PlayerData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<PlayerData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured while trying to load data from file: " + fullPath + "\n" + e);
                throw;
            }
        }

        return loadedData;
    }
    
    public WorldData LoadWorldData()
    {
        string fullPath = Path.Combine(dataDirPath, worldDataFileName);
        WorldData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<WorldData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured while trying to load data from file: " + fullPath + "\n" + e);
                throw;
            }
        }

        return loadedData;
    }

    public void SavePlayerData(PlayerData data)
    {
        string fullPath = Path.Combine(dataDirPath, playerDataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while trying to save data to file: " + fullPath + "\n" + e);
            throw;
        }
    }
    
    public void SaveWorldData(WorldData data)
    {
        string fullPath = Path.Combine(dataDirPath, worldDataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while trying to save data to file: " + fullPath + "\n" + e);
            throw;
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedData;
    }
}
