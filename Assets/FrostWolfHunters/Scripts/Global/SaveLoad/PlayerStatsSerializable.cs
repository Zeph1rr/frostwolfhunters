using System.Collections.Generic;
using UnityEngine;
using Zeph1rr.Core.Stats;

[System.Serializable]
public class PlayerStatsSerializable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStamina;
    [SerializeField] private int _damage;
    [SerializeField] private int _attackSpeed;
    [SerializeField] private int _speed;
    [SerializeField] private int _defence;
    [SerializeField] private int _critChance;
    [SerializeField] private int _critMultiplyer;

    public PlayerStatsSerializable(PlayerStats stats) {
        _maxHealth = stats.GetStatByName(PlayerStats.StatNames.MaxHealth).Level;
        _maxStamina = stats.GetStatByName(PlayerStats.StatNames.MaxStamina).Level;
        _damage = stats.GetStatByName(PlayerStats.StatNames.Damage).Level;
        _attackSpeed = stats.GetStatByName(PlayerStats.StatNames.AttackSpeed).Level;
        _speed = stats.GetStatByName(PlayerStats.StatNames.Speed).Level;
        _defence = stats.GetStatByName(PlayerStats.StatNames.Defence).Level;
        _critChance = stats.GetStatByName(PlayerStats.StatNames.CritChance).Level;
        _critMultiplyer = stats.GetStatByName(PlayerStats.StatNames.CritMultiplyer).Level;
    }

    public PlayerStats ToPlayerStats() {
        PrintStats();
        List<Stat> loadedStats = new()
        {
            new Stat(PlayerStats.StatNames.MaxHealth.ToString(), _maxHealth, 3),
            new Stat(PlayerStats.StatNames.MaxStamina.ToString(), _maxStamina, 3),
            new Stat(PlayerStats.StatNames.Damage.ToString(), _damage, 3),
            new Stat(PlayerStats.StatNames.AttackSpeed.ToString(), _attackSpeed, 3),
            new Stat(PlayerStats.StatNames.Speed.ToString(), _speed, 3),
            new Stat(PlayerStats.StatNames.Defence.ToString(), _defence, 3),
            new Stat(PlayerStats.StatNames.CritChance.ToString(), _critChance, 3),
            new Stat(PlayerStats.StatNames.CritMultiplyer.ToString(), _critMultiplyer, 3)
        };
        PlayerStats stats = new(loadedStats);
        return stats;
    }

    private void PrintStats()
    {
        Debug.Log($"_maxHealth: {_maxHealth}");
        Debug.Log($"_maxStamina: {_maxStamina}");
        Debug.Log($"_damage: {_damage}");
        Debug.Log($"_attackSpeed: {_attackSpeed}");
        Debug.Log($"_speed: {_speed}");
        Debug.Log($"_defence: {_defence}");
        Debug.Log($"_critChance: {_critChance}");
        Debug.Log($"_critMultiplyer: {_critMultiplyer}");
    }
}
