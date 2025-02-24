using UnityEngine;

[System.Serializable]
public class GameDataSerializable
{
    [SerializeField] private int _currentWaveNumber;
    [SerializeField] private int _maxWaveNumber;
    [SerializeField] private PlayerStatsSerializable _playerStats;
    [SerializeField] private string _playerName;

    public GameDataSerializable(GameData gameData, PlayerStatsSerializable playerStats) 
    {
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _playerStats = playerStats;
    }

    public GameData Deserialize(PlayerStatsSO stats) {
        GameData gameData = ScriptableObject.CreateInstance<GameData>();
        PlayerStatsSO playerStats = _playerStats.Deserialize(stats);
        gameData.Initialize(playerStats, _maxWaveNumber, _currentWaveNumber, _playerName);
        return gameData;
    }

}
