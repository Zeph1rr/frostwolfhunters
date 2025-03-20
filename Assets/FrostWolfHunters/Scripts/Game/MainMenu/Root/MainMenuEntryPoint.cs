using BaCon;
using FrostWolfHunters.Scripts.Game.GameRoot;
using FrostWolfHunters.Scripts.Game.MainMenu.Root.View;
using FrostWolfHunters.Scripts.Game.Tribe.Root;
using R3;
using UnityEngine;
using Zeph1rr.Core.SaveLoad;

namespace FrostWolfHunters.Scripts.Game.MainMenu.Root
{
    public class MainMenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private UIMainMenuRootBinder _sceneUIRootPrefab;

        public Observable<MainMenuExitParams> Run(DIContainer mainMenuContainer, MainMenuEnterParams enterParams)
        {
            MainMenuRegistrations.Register(mainMenuContainer, enterParams);
            var mainMenuViewModelsContainer = new DIContainer(mainMenuContainer);
            MainMenuViewModelsRegistrations.Register(mainMenuViewModelsContainer);
            
            ///
            
            // Для теста:
            mainMenuViewModelsContainer.Resolve<UIMainMenuRootViewModel>();
            
            var uiRoot = mainMenuContainer.Resolve<UIRoot>();
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);
            uiScene.StartScene(mainMenuContainer);
            
            var exitSignalSubj = new Subject<Unit>();
            uiScene.Bind(exitSignalSubj);

            Debug.Log($"MAIN MENU ENTRY POINT: Run main menu scene.");

            // var saveFileName = "ololo.save";
            var tribeEnterParams = new TribeEnterParams(0);
            var mainMenuExitParams = new MainMenuExitParams(tribeEnterParams);
            var exitToTribeSceneSignal = exitSignalSubj.Select(_ => mainMenuExitParams);
            
            return exitToTribeSceneSignal;
        }   
    }
}