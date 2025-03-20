﻿using System;
using System.Collections.Generic;
using System.Linq;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using UnityEngine;
using Zeph1rr.Core.Stats;

namespace FrostWolfHunters.Scripts.Game.Data
{
    [Serializable]
    public class PlayerStats
    {
        [SerializeField] private List<Stat> _stats;

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
            foreach (Stat stat in _stats)
            {
                Debug.Log($"{stat.Name}: {stat.Value}");
            }
        }

        public void InitializeEmptyStats()
        {
            _stats = new();
            foreach (string stat in Enum.GetNames(typeof(StatNames)))
            {
                _stats.Add(new Stat(stat, 0, 3));
            }
        }
    }
}
