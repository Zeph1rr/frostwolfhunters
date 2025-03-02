using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zeph1rr.Core.Stats;

public class PlayerStats
{
    public enum StatNames
    {
        MaxHealth,
        MaxStamina,
        Damage,
        Speed,
        AttackSpeed,
        Defence,
        CritChance,
        CritMultiplyer
    }

    private List<Stat> _stats = new();

    public PlayerStats()
    {
        InitializeEmptyStats();
    }

    public PlayerStats(List<Stat> stats)
    {
        _stats = stats.ToList();
    }

    public Stat GetStatByName(StatNames name)
    {
        return _stats.Find(stat => stat.Name == name.ToString());
    }

    public float GetStatValue(StatNames name)
    {
        return GetStatByName(name).Value;
    }

    public void PrintStats()
    {
        foreach(Stat stat in _stats)
        {
            Debug.Log($"{stat.Name}: {stat.Value}");
        }
    }

    public void InitializeEmptyStats()
    {
        _stats = new();
        foreach(string stat in Enum.GetNames(typeof(StatNames)))
        {
            _stats.Add(new Stat(stat, 0, 3));
        }
    }
}
