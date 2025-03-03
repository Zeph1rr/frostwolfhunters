using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TribeBuildingUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private GridLayoutGroup _shopButtonContainer;
    [SerializeField] private GameObject _shopButtonPrefab;

    private GameData _gameData;
    private string _hutName;

    private readonly Dictionary<string, PlayerStats.StatNames[]> _hutToStats = new()
    {
        {"priest_hut", new PlayerStats.StatNames[] { PlayerStats.StatNames.MaxHealth, PlayerStats.StatNames.MaxStamina }},
        {"chief_hut", new PlayerStats.StatNames[] { PlayerStats.StatNames.Damage, PlayerStats.StatNames.AttackSpeed }},
        {"shaman_hut", new PlayerStats.StatNames[] { PlayerStats.StatNames.Speed, PlayerStats.StatNames.Defence }},
        {"wolf_hut", new PlayerStats.StatNames[] { PlayerStats.StatNames.CritChance, PlayerStats.StatNames.CritMultiplyer }}
    };

    private readonly Dictionary<PlayerStats.StatNames, ResourceType> _statToResource = new()
    {
        {PlayerStats.StatNames.MaxHealth, ResourceType.Eat},
        {PlayerStats.StatNames.MaxStamina, ResourceType.Eat},
        {PlayerStats.StatNames.Damage, ResourceType.Stone},
        {PlayerStats.StatNames.AttackSpeed, ResourceType.Wood},
        {PlayerStats.StatNames.Speed, ResourceType.Bones},
        {PlayerStats.StatNames.Defence, ResourceType.Fur},
        {PlayerStats.StatNames.CritChance, ResourceType.Bones},
        {PlayerStats.StatNames.CritMultiplyer, ResourceType.Stone}
    };

    public void Initialize(string name, GameData gameData)
    {
        _hutName = name;
        _title.text = LocalizationSystem.Translate(name);
        _gameData = gameData;
        InitializeButtons();
    }

    public void CloseWindow()
    {
        Destroy(gameObject);
    }

    private void InitializeButtons()
    {
        foreach(PlayerStats.StatNames stat in _hutToStats[_hutName])
        {
            UpgradeButton buttonInstance = Instantiate(_shopButtonPrefab, _shopButtonContainer.transform).GetComponent<UpgradeButton>();
            buttonInstance.Initialize(_gameData.PlayerStats.GetStatByName(stat), _statToResource[stat], _gameData.ResourceStorage);
        }
        
    }
}
