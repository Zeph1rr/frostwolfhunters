using System;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using UnityEngine;
using Zeph1rr.Core.Resources;

namespace FrostWolfHunters.Scripts.Game.Data
{
    [Serializable]
    public class GameData
    {
        public PlayerStats PlayerStats => _playerStats;
        public int CurrentWaveNumber => _currentWaveNumber;
        public int MaxWaveNumber => _maxWaveNumber;
        public string PlayerName => _playerName;
        public ResourceStorage ResourceStorage => _resourceStorage;
        public ResourceStorage HuntResourceStorage => _huntResourceStorage;
        public bool IsDead => _isDead;
        public bool IsLeaved => _isLeaved;

        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private int _currentWaveNumber;
        [SerializeField] private int _maxWaveNumber;
        [SerializeField] private string _playerName;
        [SerializeField] private ResourceStorage _resourceStorage;

        private ResourceStorage _huntResourceStorage;
        private bool _isDead;
        private bool _isLeaved;

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

        public GameData(GameData gameData)
        {
            UpdateGameData(gameData);
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

        public void UpdateGameData(GameData gameData)
        {
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
}
