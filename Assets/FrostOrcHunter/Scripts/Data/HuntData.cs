using System;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.Data.Resource;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data
{
    [Serializable]
    public class HuntData
    {
        public int CurrentWaveNumber { get; private set; }
        public ResourceStorage ResourceStorage { get; private set; }

        public HuntData()
        {
            CurrentWaveNumber = 1;
            ResourceStorage = new ResourceStorage(Enum.GetNames(typeof(ResourceType)));
        }

        public void IncreaseWaveNumber()
        {
            CurrentWaveNumber++;
        }

        public void ResetWaveNumber()
        {
            CurrentWaveNumber = 1;
        }
    }
}