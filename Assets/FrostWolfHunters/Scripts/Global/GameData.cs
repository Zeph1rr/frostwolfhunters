using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/GameData")]
public class GameData : ScriptableObject
{
    [Header("Savable")]
    [SerializeField] private PlayerStatsSO _playerStats;
    [SerializeField] private int _currentWaveNumber = 1;
    [SerializeField] private int _maxWaveNumber = 1;
    [SerializeField] private string _playerName;
    [SerializeField] private ResourceStorage _resourceStorage;
    [Header("Unsavable")]
    [SerializeField] private ResourceStorage _huntResourceStorage;
    [SerializeField] private bool _isDead;
    [SerializeField] private bool _isLeaved;


    public PlayerStatsSO PlayerStats => _playerStats;
    public int CurrentWaveNumber => _currentWaveNumber;
    public int MaxWaveNumber => _maxWaveNumber;
    public string PlayerName => _playerName;
    public ResourceStorage ResourceStorage => _resourceStorage;
    public ResourceStorage HuntResourceStorage => _huntResourceStorage;
    public bool IsDead => _isDead;
    public bool IsLeaved => _isLeaved;

    public void Initialize(PlayerStatsSO playerStats, int maxWaveNumber, int currentWaveNumber, string playerName, ResourceStorage resourceStorage)
    {
        _playerStats = playerStats;
        _maxWaveNumber = maxWaveNumber;
        _currentWaveNumber = currentWaveNumber;
        _playerName = playerName;
        _resourceStorage = resourceStorage;
        _huntResourceStorage = new();
        _isDead = false;
        _isLeaved = false;
    }

    public void Initialize(GameData gameData) {
        _playerStats = gameData.PlayerStats;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _playerName = gameData.PlayerName;
        _resourceStorage = gameData.ResourceStorage;
        _huntResourceStorage = gameData.HuntResourceStorage;
        _isDead = gameData.IsDead;
        _isLeaved = gameData.IsLeaved;
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

    public void Leave()
    {
        _isLeaved = true;
    }

    public void Die()
    {
        _isDead = true;
    }

    public void ResetPlayerConditions()
    {
        _isLeaved = false;
        _isDead = false;
    }
}