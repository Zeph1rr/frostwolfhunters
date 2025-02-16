using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private PlayerStatsSO _playerStats;
    [SerializeField] private int _currentWaveNumber = 1;
    [SerializeField] private int _maxWaveNumber = 1;

    public PlayerStatsSO PlayerStats => _playerStats;
    public int CurrentWaveNumber => _currentWaveNumber;
    public int MaxWaveNumber => _maxWaveNumber;

    public void Initialize(PlayerStatsSO playerStats, int maxWaveNumber, int currentWaveNumber)
    {
        _playerStats = playerStats;
        _maxWaveNumber = maxWaveNumber;
        _currentWaveNumber = currentWaveNumber;
    }

    public void Initialize(GameData gameData) {
        _playerStats = gameData.PlayerStats;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _currentWaveNumber = gameData.CurrentWaveNumber;
    }

    public void IncreaseWaveNumber()
    {
        _currentWaveNumber++;
        if (_currentWaveNumber > _maxWaveNumber)
        {
            _maxWaveNumber = _currentWaveNumber;
        }
    }

    public void ResetWaveNumber()
    {
        _currentWaveNumber = 1;
    }
}