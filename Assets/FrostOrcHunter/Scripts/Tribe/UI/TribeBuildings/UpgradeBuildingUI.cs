using System.Collections.Generic;
using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.GameRoot.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostOrcHunter.Scripts.Tribe.UI.TribeBuildings
{
    public class UpgradeBuildingUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _label;
        [SerializeField] private Button _applyButton;
        
        [SerializeField] private List<UpgradeButton> _upgradeButtons;
        private GameData _gameData;
        private StatNames[] _stats;
        
        public void Initialize(string buildingName, GameData gameData, StatNames[] stats)
        {
            _applyButton.onClick.AddListener(() => Destroy(gameObject));
            _label.text = LocalizationSystem.Translate(buildingName);
            _gameData = gameData;
            _gameData.ResourceStorage.OnResourcesChanged += DrawButtons;
            _stats = stats;
            DrawButtons();
        }

        private void DrawButtons()
        {
            for (int i = 0; i < _upgradeButtons.Count; i++)
            {
                _upgradeButtons[i].Initialize(
                    _gameData.PlayerStats.GetStatByName(_stats[i]),
                    _gameData.ResourceStorage
                );
            }
        }

        private void OnDestroy()
        {
            _gameData.ResourceStorage.OnResourcesChanged -= DrawButtons;
        }
    }
}