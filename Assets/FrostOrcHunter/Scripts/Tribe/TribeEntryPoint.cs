using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.GameRoot.UI;
using FrostOrcHunter.Scripts.Tribe.RandomEvents;
using FrostOrcHunter.Scripts.Tribe.UI;
using UnityEngine;
using Zeph1rrGameBase.Scripts.Core.DI;
using Zeph1rrGameBase.Scripts.Core.Scene;

namespace FrostOrcHunter.Scripts.Tribe
{
    public class TribeEntryPoint : MonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] private TribeUIRoot _uiScenePrefab;
        
        private DIContainer _tribeContainer;
        private GameData _gameData;
        private TribeUIRoot _uiScene;
        
        public void Initialize(DIContainer rootContainer)
        {
            _tribeContainer = new DIContainer(rootContainer);
            _uiScene = Instantiate(_uiScenePrefab);
            var uiRoot = _tribeContainer.Resolve<UIRoot>();
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            _uiScene.Initialize(_tribeContainer);
        }

        public void Run()
        {
            Debug.Log("Tribe Entry");
            _gameData = _tribeContainer.Resolve<GameData>();
            // var randomEvent = new HungryTribeEvent("Племя голодно", "Спиздили 10% еды");
            // randomEvent.Run(_gameData);
            Debug.Log(JsonUtility.ToJson(_gameData));
        }
    }
}