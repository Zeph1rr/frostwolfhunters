using FrostOrcHunter.Scripts.GameRoot.UI;
using FrostOrcHunter.Scripts.MainMenu.UI;
using UnityEngine;
using Zeph1rrGameBase.Scripts.Core.DI;
using Zeph1rrGameBase.Scripts.Core.Scene;

namespace FrostOrcHunter.Scripts.MainMenu
{
    public class MainMenuEntryPoint : MonoBehaviour, ISceneEntryPoint
    {
        [SerializeField] private MainMenuUIRoot _uiScenePrefab;
        
        private DIContainer _mainMenuContainer;
        private MainMenuUIRoot _uiScene;
        
        public void Initialize(DIContainer rootContainer)
        {
            _mainMenuContainer = rootContainer;
            _uiScene = Instantiate(_uiScenePrefab);
            var uiRoot = _mainMenuContainer.Resolve<UIRoot>();
            uiRoot.AttachSceneUI(_uiScene.gameObject);
            _uiScene.Initialize(rootContainer);
        }

        public void Run()
        {
            Debug.Log("Main Menu Entry");
        }
    }
}