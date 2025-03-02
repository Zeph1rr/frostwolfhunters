using UnityEngine;

namespace Zeph1rr.Core.Stats
{
    [System.Serializable]
    public class Stat
    {
        private int _level;
        private string _name;
        private float _currentValue;
        private int _baseCost;

        public string Name => _name;
        public float Value => _currentValue;
        public int Level => _level;

        public Stat(string name, int level, int baseCost)
        {
            _name = name;
            _level = level;
            _currentValue = StatsLoader.GetStatValue(_name, _level);
            _baseCost = baseCost;
        }

        public Stat(int level, string name, float value, int baseCost)
        {
            _level = level;
            _name = name;
            _currentValue = value;
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
            _level += 1;
            _currentValue = StatsLoader.GetStatValue(_name, _level);
        }

        public void ChangeValue(float value)
        {
            _currentValue += value;
        }
    }
}