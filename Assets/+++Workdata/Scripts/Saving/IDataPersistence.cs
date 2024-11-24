using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadPlayerData(PlayerData playerData);
    void LoadWorldData(WorldData worldData);

    void SaveData(ref PlayerData playerData, ref WorldData worldData);
}
