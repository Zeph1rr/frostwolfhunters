using System.Collections.Generic;
using BaCon;
using FrostWolfHunters.Scripts.Game.GameRoot;
using FrostWolfHunters.Scripts.Game.MainMenu.Root;
using FrostWolfHunters.Scripts.Game.Tribe.Root.View;
using R3;
using UnityEngine;

namespace FrostWolfHunters.Scripts.Game.Tribe.Root
{
    public class TribeEntryPoint : MonoBehaviour
    {
        [SerializeField] private UITribeRootBinder _sceneUIRootPrefab;

        private Dictionary<string, TribeExitParams> _exitParamsBySceneName = new Dictionary<string, TribeExitParams>()
        {
            { Scenes.MAIN_MENU, new TribeExitParams(new MainMenuEnterParams()) },
            { Scenes.HUNT, null }
        };

        public Observable<TribeExitParams> Run(DIContainer tribeCotainer, TribeEnterParams enterParams)
        {
            TribeRegistrations.Register(tribeCotainer, enterParams);
            var tribeViewModelsContainer = new DIContainer(tribeCotainer);
            TribeViewModelsRegistrations.Register(tribeViewModelsContainer);
            
            tribeViewModelsContainer.Resolve<UITribeRootViewModel>();
            
            var uiRoot = tribeCotainer.Resolve<UIRoot>();
            var uiScene = Instantiate(_sceneUIRootPrefab);
            uiRoot.AttachSceneUI(uiScene.gameObject);
            uiScene.StartScene(tribeCotainer);
            
            var exitSignalSubj = new Subject<string>();
            uiScene.Bind(exitSignalSubj);
            
            var exitToTribeSceneSignal = exitSignalSubj.Select(sceneName => _exitParamsBySceneName[sceneName]);
            
            return exitToTribeSceneSignal;
        }
    }
}