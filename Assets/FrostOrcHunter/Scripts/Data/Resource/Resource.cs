using System;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data.Resource
{
    [Serializable]
    public class Resource
    {
        [SerializeField] private string _name;
        [SerializeField] private int _value;
        
        public string Name => _name;
        public int Value => _value;

        public Resource(string name, int value)
        {
            _name = name;
            _value = value;
        }

        public void IncreaseValue(int value)
        {
            if (value < 0) 
            {
                throw new ArgumentOutOfRangeException("Value cannot be negative!");
            }
            _value += value;
        }

        public void DecreaseValue(int value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Value cannot be negative!");
            }
            if (value > _value)
            {
                throw new ArgumentOutOfRangeException($"You don't have enough {_name}");
            }
            _value -= value;
        }

        public void DecreaseByMultiplyer(float multyiplier)
        {
            if (multyiplier < 0 || multyiplier > 1)
            {
                throw new ArgumentOutOfRangeException("Value should be between 0 and 1!");
            }
            _value = Mathf.FloorToInt(_value * multyiplier);
        }
    }
}