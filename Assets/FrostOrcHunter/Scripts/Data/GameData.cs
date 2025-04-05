using System;
using System.Collections.Generic;
using System.Linq;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.Data.Resource;
using FrostOrcHunter.Scripts.Data.Stats;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data
{
    [Serializable]
    public class GameData
    {
        public Action OnCampBuildingsAdd;
        public Action OnFirstHunt;
        
        public PlayerStats PlayerStats => _playerStats;
        public int MaxWaveNumber => _maxWaveNumber;
        public string PlayerName => _playerName;
        public ResourceStorage ResourceStorage => _resourceStorage;
        public List<CampBuildings> CampBuildings => _campBuildings;
        public List<CampBuildings> PressedBuildings => _pressedBuildings;
        public HuntData HuntData => _huntData;
        public bool IsDead => _isDead;
        public bool IsLeaved => _isLeaved;

        public bool IsFirstHunt => _isFirstHunt;
        
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private int _maxWaveNumber;
        [SerializeField] private string _playerName;
        [SerializeField] private ResourceStorage _resourceStorage;
        [SerializeField] private List<CampBuildings> _campBuildings;
        [SerializeField] private List<CampBuildings> _pressedBuildings;
        [SerializeField] private bool _isFirstHunt;
        
        private HuntData _huntData;
        private bool _isDead;
        private bool _isLeaved;
        
        
        public GameData()
        {
            _playerStats = new PlayerStats();
            _maxWaveNumber = 1;
            _playerName = "hunter";
            _resourceStorage = new ResourceStorage(Enum.GetNames(typeof(ResourceType)));
            _campBuildings = new List<CampBuildings>() { Enums.CampBuildings.Firepit, Enums.CampBuildings.HunterHut, Enums.CampBuildings.WallDoor};
            _pressedBuildings = new List<CampBuildings>();
            _huntData = new HuntData();
            _isDead = false;
            _isLeaved = false;
            _isFirstHunt = true;
        }

        public GameData(PlayerStats playerStats, int maxWaveNumber, int currentWaveNumber, string playerName, ResourceStorage resourceStorage, List<CampBuildings> campBuildings, List<CampBuildings> pressedBuildings, bool isDead, bool isLeaved, bool isFirstHunt)
        {
            _playerStats = playerStats;
            _maxWaveNumber = maxWaveNumber;
            _playerName = playerName;
            _resourceStorage = resourceStorage;
            _campBuildings = campBuildings;
            _pressedBuildings = pressedBuildings;
            _huntData = new HuntData();
            _isDead = false;
            _isLeaved = false;
            _isFirstHunt = isFirstHunt;
        }

        public GameData(GameData gameData)
        {
            UpdateGameData(gameData);
        }

        public GameData(string playerName)
        {
            _playerStats = new PlayerStats();
            _maxWaveNumber = 1;
            _playerName = playerName;
            _resourceStorage = new ResourceStorage(Enum.GetNames(typeof(ResourceType)));
            _campBuildings = new List<CampBuildings>() { Enums.CampBuildings.Firepit, Enums.CampBuildings.HunterHut, Enums.CampBuildings.WallDoor};
            _pressedBuildings = new List<CampBuildings>();
            _huntData = new HuntData();
            _isDead = false;
            _isLeaved = false;
            _isFirstHunt = true;
        }
        
        public void UpdateGameData(GameData gameData)
        {
            _playerStats = gameData.PlayerStats;
            _maxWaveNumber = gameData.MaxWaveNumber;
            _playerName = gameData.PlayerName;
            _resourceStorage = gameData.ResourceStorage;
            _campBuildings = gameData.CampBuildings;
            _pressedBuildings = gameData.PressedBuildings;
            _huntData = gameData.HuntData;
            _isDead = gameData.IsDead;
            _isLeaved = gameData.IsLeaved;
            _isFirstHunt = gameData.IsFirstHunt;
        }

        public void IncreaseWaveNumber()
        {
            _huntData.IncreaseWaveNumber();
            if (_huntData.CurrentWaveNumber > _maxWaveNumber) _maxWaveNumber = _huntData.CurrentWaveNumber;
        }

        public void ResetWaveNumber()
        {
            _huntData.ResetWaveNumber();
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

        public void AddBuilding(CampBuildings campBuilding)
        {
            if (_campBuildings.Contains(campBuilding))
            {
                Debug.Log($"{campBuilding} already present");
                return;
            }
            _campBuildings.Add(campBuilding);
            OnCampBuildingsAdd?.Invoke();
        }

        public void FirstHunt()
        {
            _isFirstHunt = false;
            OnFirstHunt?.Invoke();
        }

        public void PressBuilding(CampBuildings campBuilding)
        {
            if (_pressedBuildings.Contains(campBuilding))
            {
                return;
            }
            _pressedBuildings.Add(campBuilding);
        }
    }
}