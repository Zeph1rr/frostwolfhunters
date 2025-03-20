using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using Zeph1rr.Core.Localization;

public class TribeBuildingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GridLayoutGroup _shopButtonContainer;
    [SerializeField] private GameObject _shopButtonPrefab;

    private GameData _gameData;
    private string _hutName;

    private readonly Dictionary<string, StatNames[]> _hutToStats = new()
    {
        {"priest_hut", new StatNames[] { StatNames.MaxHealth, StatNames.MaxStamina }},
        {"chief_hut", new StatNames[] { StatNames.Damage, StatNames.AttackSpeed }},
        {"shaman_hut", new StatNames[] { StatNames.Speed, StatNames.Defence }},
        {"wolf_hut", new StatNames[] { StatNames.CritChance, StatNames.CritMultiplyer }}
    };

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

    public void Initialize(string name, GameData gameData)
    {
        _hutName = name;
        _title.text = LocalizationSystem.Translate(name);
        _gameData = gameData;
        InitializeButtons();
        _gameData.ResourceStorage.OnResourcesChanged += HandleResourceChanged;
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _gameData.ResourceStorage.OnResourcesChanged -= HandleResourceChanged;
    }

    private void HandleResourceChanged(object sender, EventArgs e)
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        foreach (Transform child in _shopButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(StatNames stat in _hutToStats[_hutName])
        {
            UpgradeButton buttonInstance = Instantiate(_shopButtonPrefab, _shopButtonContainer.transform).GetComponent<UpgradeButton>();
            buttonInstance.Initialize(_gameData.PlayerStats.GetStatByName(stat), _statToResource[stat], _gameData.ResourceStorage);
        }
        
    }
}
