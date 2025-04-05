using System;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.Data.Stats;
using FrostOrcHunter.Scripts.GameRoot.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostOrcHunter.Scripts.Tribe.UI.TribeBuildings
{
    public class HunterHutUI : MonoBehaviour
    {
        [SerializeField] private Transform _statsContainer;

        public void Initialize(PlayerStats playerStats)
        {
            InitializeStats(playerStats);
            var button = GetComponentInChildren<Button>();
            button.onClick.AddListener(() => Destroy(gameObject));
        }

        private void InitializeStats(PlayerStats playerStats)
        {
            foreach (var stat in Enum.GetNames(typeof(StatNames)))
            {
                var statUI = Instantiate(Resources.Load<TextMeshProUGUI>("Prefabs/UI/Tribe/Stat"), _statsContainer);
                var statName = LocalizationSystem.Translate(stat);
                var statValue = playerStats.GetStatValue(Enum.Parse<StatNames>(stat));
                statUI.text = $"{statName}: {statValue}";
            }
            
        }
    }
}