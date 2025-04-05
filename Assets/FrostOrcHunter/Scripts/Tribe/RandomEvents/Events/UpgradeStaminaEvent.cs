using System;
using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.Data.Enums;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Tribe.RandomEvents.Events
{
    public class UpgradeStaminaEvent : RandomEvent
    {
        public UpgradeStaminaEvent(string title, string description, string onFailureExpand, string onSuccessExpand) : base(title, description, onFailureExpand, onSuccessExpand)
        {
        }

        public override void Run(GameData gameData)
        {
            var failure = false;
            try
            {
                var stat = gameData.PlayerStats.GetStatByName(StatNames.MaxStamina);
                stat.Upgrade();
            }
            catch (ArgumentOutOfRangeException e)
            {
                Debug.Log(e);
                failure = true;
            }
            Draw(failure);
        }
    }
}