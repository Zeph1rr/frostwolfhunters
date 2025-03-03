using System;
using Zeph1rr.Core.Resources;

public class GameData
{
    private PlayerStats _playerStats;
    private int _currentWaveNumber;
    private int _maxWaveNumber;
    private string _playerName;
    private ResourceStorage _resourceStorage;
    private ResourceStorage _huntResourceStorage;
    private bool _isDead;
    private bool _isLeaved;
    
    public PlayerStats PlayerStats => _playerStats;
    public int CurrentWaveNumber => _currentWaveNumber;
    public int MaxWaveNumber => _maxWaveNumber;
    public string PlayerName => _playerName;
    public ResourceStorage ResourceStorage => _resourceStorage;
    public ResourceStorage HuntResourceStorage => _huntResourceStorage;
    public bool IsDead => _isDead;
    public bool IsLeaved => _isLeaved;

    public GameData()
    {
        _playerStats = new();
        _maxWaveNumber = 1;
        _currentWaveNumber = 1;
        _playerName = "hunter";
        _resourceStorage = new(Enum.GetNames(typeof(ResourceType)));
        _huntResourceStorage = new(Enum.GetNames(typeof(ResourceType)));
        _isDead = false;
        _isLeaved = false;
    }

    public GameData(PlayerStats playerStats, int maxWaveNumber, int currentWaveNumber, string playerName, ResourceStorage resourceStorage)
    {
        _playerStats = playerStats;
        _maxWaveNumber = maxWaveNumber;
        _currentWaveNumber = currentWaveNumber;
        _playerName = playerName;
        _resourceStorage = resourceStorage;
        _huntResourceStorage = new(Enum.GetNames(typeof(ResourceType)));
        _isDead = false;
        _isLeaved = false;
    }

    public GameData(GameData gameData) {
        _playerStats = gameData.PlayerStats;
        _maxWaveNumber = gameData.MaxWaveNumber;
        _currentWaveNumber = gameData.CurrentWaveNumber;
        _playerName = gameData.PlayerName;
        _resourceStorage = gameData.ResourceStorage;
        _huntResourceStorage = gameData.HuntResourceStorage;
        _isDead = gameData.IsDead;
        _isLeaved = gameData.IsLeaved;
    }

    public GameData(string playerName)
    {
        _playerStats = new();
        _maxWaveNumber = 1;
        _currentWaveNumber = 1;
        _playerName = playerName;
        _resourceStorage = new(Enum.GetNames(typeof(ResourceType)));
        _huntResourceStorage = new(Enum.GetNames(typeof(ResourceType)));
        _isDead = false;
        _isLeaved = false;
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

    public GameDataSerializable ToGameDataSerializable()
    {
        return new(this, new PlayerStatsSerializable(_playerStats));
    }
}
