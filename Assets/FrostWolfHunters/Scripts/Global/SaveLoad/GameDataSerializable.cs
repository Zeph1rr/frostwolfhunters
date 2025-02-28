using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class GameDataSerializable
{
    [SerializeField] private int _currentWaveNumber;
    [SerializeField] private int _maxWaveNumber;
    [SerializeField] private PlayerStatsSerializable _playerStats;
    [SerializeField] private string _playerName;
    [SerializeField] private List<KeyValuePair<string, int>> _resources;

    public GameDataSerializable(GameData gameData, PlayerStatsSerializable playerStats) 
    {
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _playerStats = playerStats;
        _playerName = gameData.PlayerName;
        Dictionary<string,int> resources = gameData.ResourceStorage.Resources.ToDictionary(entry => entry.Key, entry => entry.Value);
        _resources = new List<KeyValuePair<string, int>>(resources);
    }

    public GameData Deserialize(PlayerStatsSO stats) {
        GameData gameData = ScriptableObject.CreateInstance<GameData>();
        PlayerStatsSO playerStats = _playerStats.Deserialize(stats);
        ResourceStorage resourceStorage = new();
        resourceStorage.AddResources(_resources.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        gameData.Initialize(playerStats, _maxWaveNumber, _currentWaveNumber, _playerName, resourceStorage);
        return gameData;
    }

}
