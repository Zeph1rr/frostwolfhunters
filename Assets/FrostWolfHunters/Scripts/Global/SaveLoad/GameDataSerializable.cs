using UnityEngine;
using Zeph1rr.Core.Recources;
using Zeph1rr.Core.SaveLoad;

[System.Serializable]
public class GameDataSerializable: ISerializableData<GameData>
{
    [SerializeField] private int _currentWaveNumber;
    [SerializeField] private int _maxWaveNumber;
    [SerializeField] private PlayerStatsSerializable _playerStats;
    [SerializeField] private string _playerName;
    [SerializeField] private ResourcesSerialazable _resources;

    public GameDataSerializable(GameData gameData, PlayerStatsSerializable playerStats) 
    {
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _playerStats = playerStats;
        _playerName = gameData.PlayerName;
        _resources = new(gameData.ResourceStorage);
    }

    public GameData Deserialize() {
        PlayerStats playerStats = _playerStats.ToPlayerStats();
        ResourceStorage resourceStorage = _resources.ToResourceStorage();
        GameData gameData = new(playerStats, _maxWaveNumber, _currentWaveNumber, _playerName, resourceStorage);
        return gameData;
    }

}
