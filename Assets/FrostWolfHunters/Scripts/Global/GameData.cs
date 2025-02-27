using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/GameData")]
public class GameData : ScriptableObject
{
    [SerializeField] private PlayerStatsSO _playerStats;
    [SerializeField] private int _currentWaveNumber = 1;
    [SerializeField] private int _maxWaveNumber = 1;
    [SerializeField] private string _playerName;
    [SerializeField] private ResourceStorage _resourceStorage;

    public PlayerStatsSO PlayerStats => _playerStats;
    public int CurrentWaveNumber => _currentWaveNumber;
    public int MaxWaveNumber => _maxWaveNumber;
    public string PlayerName => _playerName;
    public ResourceStorage ResourceStorage => _resourceStorage;

    public void Initialize(PlayerStatsSO playerStats, int maxWaveNumber, int currentWaveNumber, string playerName, ResourceStorage resourceStorage)
    {
        _playerStats = playerStats;
        _maxWaveNumber = maxWaveNumber;
        _currentWaveNumber = currentWaveNumber;
        _playerName = playerName;
        _resourceStorage = resourceStorage;
    }

    public void Initialize(GameData gameData) {
        _playerStats = gameData.PlayerStats;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _playerName = gameData.PlayerName;
        _resourceStorage = gameData.ResourceStorage;
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