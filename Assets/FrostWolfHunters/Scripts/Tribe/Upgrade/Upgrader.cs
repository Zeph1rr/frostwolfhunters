using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Upgrader: MonoBehaviour
{
    [SerializeField] PlayerStatsSO _playerStats;
    [SerializeField] PlayerStatsSO _basePlayerStats;
    [SerializeField] GameData _gameData;
    [SerializeField] private int _baseCost = 3;
    public Dictionary<string, ResourceType> ResourceTypeByStat = new()
    {
        {"MaxHealth", ResourceType.Eat},
        {"MaxStamina", ResourceType.Eat},
        {"Damage", ResourceType.Stone},
        {"AttackSpeed", ResourceType.Bones},
        {"Speed", ResourceType.Wood},
        {"Defence", ResourceType.Fur}
    };

    public int CalculateCost(int statLevel)
    {
        return Mathf.RoundToInt(Mathf.Pow(1.1f, statLevel) * _baseCost);
    }

    public float CalculateNextStatValue(float currentStat)
    {
        return currentStat + 5f;
    }

    public int GetCurrentStatLevel(float baseStat, float currentStat)
    {
        return Mathf.RoundToInt((currentStat - baseStat) / 5);
    }

    public void UpgradeDefence()
    {
        string resourceType = ResourceTypeByStat["Defence"].ToString();
        int statLevel = GetCurrentStatLevel(_basePlayerStats.Defence, _playerStats.Defence);
        Debug.Log(statLevel);
        bool spendResult = _gameData.ResourceStorage.TrySpendResource(resourceType, CalculateCost(statLevel));
        Debug.Log(spendResult);
        if (!spendResult) return;
        _playerStats.Defence = Mathf.RoundToInt(CalculateNextStatValue(_playerStats.Defence));
    }
}
