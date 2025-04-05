using System;
using System.Collections.Generic;
using System.Linq;
using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.Data.Enums;
using FrostOrcHunter.Scripts.GameRoot;
using FrostOrcHunter.Scripts.Tribe.UI.TribeBuildings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zeph1rrGameBase.Scripts.Core.DI;
using Zeph1rrGameBase.Scripts.Core.Utils;

namespace FrostOrcHunter.Scripts.Tribe.UI
{
    [RequireComponent(typeof(Button))]
    public class TribeBuilding : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _mark;
        
        private TribeUIRoot _uiRoot;
        private GameData _gameData;
        
        private readonly Dictionary<string, StatNames[]> _hutToStats = new()
        {
            {CampBuildings.PriestHut.ToString(), new[] { StatNames.MaxHealth, StatNames.MaxStamina, StatNames.CritMultiplyer, StatNames.CritChance }},
            {CampBuildings.ShamanHut.ToString(), new[] { StatNames.Speed, StatNames.Defence, StatNames.Damage, StatNames.AttackSpeed }},
        };
        
        public void Initialize(DIContainer container)
        {
            _uiRoot = container.Resolve<TribeUIRoot>();
            _gameData = container.Resolve<GameData>();
            InitializeButton();
        }
        
        private void InitializeButton()
        {
            var button = GetComponent<Button>();
            button.onClick.RemoveAllListeners();

            button.onClick.AddListener(() => _gameData.PressBuilding(Enum.Parse<CampBuildings>(gameObject.name)));
            
            if (_hutToStats.Keys.ToList().Contains(gameObject.name))
            {
                _mark.gameObject.SetActive(!_gameData.PressedBuildings.Contains(Enum.Parse<CampBuildings>(gameObject.name)));
                button.onClick.AddListener(CreateUpgradeUI);
            }
                
            
            if (gameObject.name == CampBuildings.Firepit.ToString())
            {
                button.onClick.AddListener(RunRandomEvent);
                if (_uiRoot.RandomEvent != null)
                    _mark.gameObject.SetActive(true);
            }

            if (gameObject.name == CampBuildings.ChiefHut.ToString())
            {
                _mark.gameObject.SetActive(_gameData.IsFirstHunt);
                button.onClick.AddListener(ChiefHutEvent);
            }

            if (gameObject.name == CampBuildings.WallDoor.ToString())
            {
                _mark.gameObject.SetActive(!_gameData.PressedBuildings.Contains(Enum.Parse<CampBuildings>(gameObject.name)));
                button.onClick.AddListener(WallDoorEvent);
            }

            if (gameObject.name == CampBuildings.HunterHut.ToString())
            {
                _mark.gameObject.SetActive(!_gameData.PressedBuildings.Contains(Enum.Parse<CampBuildings>(gameObject.name)));
                button.onClick.AddListener(CreateHunterHutUi);
            }
        }

        private void ChiefHutEvent()
        {
            if (_gameData.IsFirstHunt)
            {
                _gameData.FirstHunt();
                _mark.gameObject.SetActive(false);
            }

            _gameData.AddBuilding(CampBuildings.PriestHut);
        }

        private void WallDoorEvent()
        {
            if (_gameData.IsFirstHunt)
            {
                _gameData.AddBuilding(CampBuildings.ChiefHut);
                _mark.gameObject.SetActive(false);
            }
            
            Coroutines.StartRoutine(GameEntryPoint.Instance.LoadScene(Scenes.TRIBE));
        }

        private void RunRandomEvent()
        {
            _uiRoot.RandomEvent?.Run(_gameData);
            _mark.gameObject.SetActive(false);
            _uiRoot.RemoveRandomEvent();
        }

        private void CreateHunterHutUi()
        {
            if (_gameData.IsFirstHunt)
            {
                _mark.gameObject.SetActive(false);
            }
            var ui = Instantiate(Resources.Load<HunterHutUI>("Prefabs/UI/Tribe/HunterHutUI"), _uiRoot.transform);
            ui.Initialize(_gameData.PlayerStats);
        }

        private void CreateUpgradeUI()
        {
            _mark.gameObject.SetActive(false);
            var ui = Instantiate(Resources.Load<UpgradeBuildingUI>("Prefabs/UI/Tribe/UpgradeBuildingUI"),_uiRoot.transform);
            ui.Initialize(gameObject.name, _gameData, _hutToStats[gameObject.name]);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1.15f, 1.15f, 1f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}