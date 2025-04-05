using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.Tribe.UI;
using FrostOrcHunter.Scripts.Tribe.UI.TribeBuildings;
using UnityEngine;
using UnityEngine.Rendering;

namespace FrostOrcHunter.Scripts.Tribe.RandomEvents
{
    public abstract class RandomEvent
    {
        public string Title => _title;
        public string Description => _description;

        private string _title;
        private string _description;
        
        protected string OnFailureExpand;
        protected string OnSuccessExpand;

        public RandomEvent(string title, string description, string onFailureExpand, string onSuccessExpand)
        {
            _title = title;
            _description = description;
            OnFailureExpand = onFailureExpand;
            OnSuccessExpand = onSuccessExpand;
        }

        public abstract void Run(GameData gameData);

        protected void Draw(bool failure)
        {
            var eventPrefab = Resources.Load<RandomEventUI>("Prefabs/UI/Tribe/RandomEventUI");
            var tribeUIRoot = Object.FindFirstObjectByType<TribeUIRoot>();
            var eventUI = Object.Instantiate(eventPrefab, tribeUIRoot.transform);
            var expand = failure ? OnFailureExpand : OnSuccessExpand;
            eventUI.Initialize(_title, _description + expand);
        }
    }
}