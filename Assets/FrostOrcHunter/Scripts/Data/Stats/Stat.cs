using System;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data.Stats
{
    [System.Serializable]
    public class Stat
    {
        private int _level;
        private string _name;
        private int _baseCost;

        public string Name => _name;
        public float Value => StatsLoader.GetStatValue(_name, _level);
        public int Level => _level;

        public Stat(string name, int level, int baseCost)
        {
            _name = name;
            _level = level;
            _baseCost = baseCost;
        }

        public Stat(int level, string name, float value, int baseCost)
        {
            _level = level;
            _name = name;
            _baseCost = baseCost;
        }

        public float GetNextValue()
        {
            return StatsLoader.GetStatValue(_name, _level + 1);
        }       

        public int GetNextValueCost()
        {
            return Mathf.RoundToInt(_baseCost * Mathf.Pow(1.15f, _level));
        }

        public void Upgrade()
        {
            GetNextValue();
            _level += 1;
        }
    }
}