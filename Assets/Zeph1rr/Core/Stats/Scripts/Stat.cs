using UnityEngine;

namespace Zeph1rr.Core.Stats
{
    [System.Serializable]
    public class Stat
    {
        [SerializeField] private int _level;
        [SerializeField] private string _name;
        [SerializeField] private int _baseCost;

        public string Name => _name;
        public float Value => StatsLoader.GetStatValue(_name, _level);
        public int Level => _level;

        public Stat(string name, int level, int baseCost)
        {
            _name = name;
            _level = level;
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
        }
    }
}