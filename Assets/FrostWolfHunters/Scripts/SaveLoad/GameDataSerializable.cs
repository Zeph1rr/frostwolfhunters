using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameDataSerializable
{
    [SerializeField] private int _currentWaveNumber;
    [SerializeField] private int _maxWaveNumber;
    [SerializeField] private PlayerStatsSerializable _playerStats;
    [SerializeField] private string _playerName;
    [SerializeField] private Dictionary<string,int> _resources;

    public GameDataSerializable(GameData gameData, PlayerStatsSerializable playerStats) 
    {
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _playerStats = playerStats;
        _playerName = gameData.PlayerName;
        _resources = gameData.ResourceStorage.Resources;
    }

    public GameData Deserialize(PlayerStatsSO stats) {
        GameData gameData = ScriptableObject.CreateInstance<GameData>();
        PlayerStatsSO playerStats = _playerStats.Deserialize(stats);
        ResourceStorage resourceStorage = new();
        resourceStorage.AddResources(_resources);
        gameData.Initialize(playerStats, _maxWaveNumber, _currentWaveNumber, _playerName, resourceStorage);
        return gameData;
    }

}
