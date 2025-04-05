using System;
using System.Collections.Generic;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.Data.Resource;
using FrostOrcHunter.Scripts.Data.Stats;
using FrostOrcHunter.Scripts.GameRoot.Localization;
using FrostOrcHunter.Scripts.GameRoot.UI.Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrostOrcHunter.Scripts.Tribe.UI
{
    [RequireComponent(typeof(Button))]
    public class UpgradeButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private ResourceView _resourceView;
        
        private Stat _stat;
        private Resource _resource;
        private Button _button;
        private ResourceStorage _resourceStorage;
        
        private readonly Dictionary<StatNames, ResourceType> _statToResource = new()
        {
            {StatNames.MaxHealth, ResourceType.Eat},
            {StatNames.MaxStamina, ResourceType.Eat},
            {StatNames.Damage, ResourceType.Stone},
            {StatNames.AttackSpeed, ResourceType.Wood},
            {StatNames.Speed, ResourceType.Bones},
            {StatNames.Defence, ResourceType.Fur},
            {StatNames.CritChance, ResourceType.Bones},
            {StatNames.CritMultiplyer, ResourceType.Stone}
        };

        public void Initialize(Stat stat, ResourceStorage resourceStorage)
        {
            _button = GetComponent<Button>();
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(UpgradeStat);
            
            _stat = stat;
            _resourceStorage = resourceStorage;
            
            var resourceName = (StatNames) Enum.Parse(typeof(StatNames), stat.Name);
            var resourceType = _statToResource[resourceName].ToString();
            _resource = new Resource(resourceType, _stat.GetNextValueCost());
            
            try
            {
                var nextValue = _stat.GetNextValue();
                _label.text = $"{LocalizationSystem.Translate(stat.Name)} ({_stat.Value} -> {nextValue})";
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log(e);
                _label.text = $"{LocalizationSystem.Translate(_stat.Name)} ({_stat.Value} MAX)";
                _button.interactable = false;
            }
            
            if (_resourceStorage.GetResourceValueByName(resourceType) < _stat.GetNextValueCost())
            {
                _button.interactable = false;
                _resourceView.SetTextColor(Color.red);
                _resourceView.Initialize(_resource);
                return;
            }
            
            _button.interactable = true;
            _resourceView.SetTextColor(Color.white);
            _resourceView.Initialize(_resource);
        }

        private void UpgradeStat()
        {
            _resourceStorage.Buy(_resource.Name, _resource.Value, () => _stat.Upgrade());
        }
    }
}