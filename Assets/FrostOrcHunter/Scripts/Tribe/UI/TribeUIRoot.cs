using System;
using System.Collections.Generic;
using System.Linq;
using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.GameRoot.UI.Resource;
using FrostOrcHunter.Scripts.Tribe.RandomEvents;
using FrostOrcHunter.Scripts.Tribe.RandomEvents.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using Zeph1rrGameBase.Scripts.Core.DI;
using Random = UnityEngine.Random;

namespace FrostOrcHunter.Scripts.Tribe.UI
{
    public class TribeUIRoot : MonoBehaviour
    {
        [SerializeField] private TribeMenu _tribeMenu;
        [SerializeField] private ResourceStorageView _resourceStorageView;
        [SerializeField] private List<TribeBuilding> _buildings;
        
        public RandomEvent RandomEvent { get; private set; }
        
        private InputActions _inputActions;
        private GameData _gameData;
        private DIContainer _container;

        private List<RandomEvent> _randomEvents;
        
        public void Initialize(DIContainer container)
        {
            _container = container;
            
            _gameData = _container.Resolve<GameData>();
            _gameData.OnCampBuildingsAdd += InitializeTribeBuildings;
            _gameData.OnFirstHunt += InitializeTribeBuildings;
            
            _randomEvents = new List<RandomEvent>
            {
                RandomEventSystem.CreateEvent<HungryTribeEvent>(),
                RandomEventSystem.CreateEvent<UpgradeStaminaEvent>(),
            };
            
            _tribeMenu.Initialize(_container);
            _container.RegisterInstance(this);
            
            _inputActions = _container.Resolve<InputActions>();
            _inputActions.Enable();
            _inputActions.Global.Escape.performed += ToggleTribeMenu;

            if (Random.Range(0, 100) < 30)
            {
                RandomEvent = _randomEvents[Random.Range(0, _randomEvents.Count)];
                Debug.Log($"RandomEvent: {RandomEvent}");
            }

            InitializeTribeBuildings();
            
            _resourceStorageView.Initialize(_gameData.ResourceStorage);
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
            _inputActions.Global.Escape.performed -= ToggleTribeMenu;
            _gameData.OnCampBuildingsAdd -= InitializeTribeBuildings;
            _gameData.OnFirstHunt -= InitializeTribeBuildings;
        }

        private void InitializeTribeBuildings()
        {
            foreach (var tribeBuilding in _buildings)
            {
                tribeBuilding.gameObject.SetActive(false);
                if (_gameData.CampBuildings.Contains((CampBuildings)Enum.Parse(typeof(CampBuildings), tribeBuilding.name)))
                {
                    tribeBuilding.gameObject.SetActive(true);
                    tribeBuilding.Initialize(_container);
                }
            }
        }

        private void ToggleTribeMenu(InputAction.CallbackContext obj)
        {
            _tribeMenu.Toggle();
        }

        public void RemoveRandomEvent()
        {
            RandomEvent = null;
        }
    }
}