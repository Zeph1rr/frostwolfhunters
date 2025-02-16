using UnityEngine;

[System.Serializable]
public class GameDataSerializable
{
    [SerializeField] private int _currentWaveNumber;
    [SerializeField] private int _maxWaveNumber;
    [SerializeField] private PlayerStatsSerializable _playerStats;

    public GameDataSerializable(GameData gameData, PlayerStatsSerializable playerStats) 
    {
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _playerStats = playerStats;
    }

    public GameData Deserialize() {
        GameData gameData = ScriptableObject.CreateInstance<GameData>();
        PlayerStatsSO playerStats = _playerStats.Deserialize();
        gameData.Initialize(playerStats, _maxWaveNumber, _currentWaveNumber);
        return gameData;
    }

}
