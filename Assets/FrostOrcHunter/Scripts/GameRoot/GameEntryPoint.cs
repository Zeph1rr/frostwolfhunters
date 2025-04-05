using System;
using System.Collections;
using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.GameRoot.UI;
using UnityEngine;
using Zeph1rrGameBase.Scripts.Core.DI;
using Zeph1rrGameBase.Scripts.Core.SaveLoadSystem;
using Zeph1rrGameBase.Scripts.Core.Scene;
using Zeph1rrGameBase.Scripts.Core.Utils;
using Object = UnityEngine.Object;

namespace FrostOrcHunter.Scripts.GameRoot
{
    public class GameEntryPoint
    {
        public static GameEntryPoint Instance;
        private readonly UIRoot _uiRoot;
        private readonly DIContainer _rootContainer;
        private Scene _currentScene;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoStartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Instance ??= new GameEntryPoint();
            Instance.RunGame();
        }

        private GameEntryPoint()
        {
            _rootContainer = new DIContainer();
            var prefabUIRoot = Resources.Load<UIRoot>("Prefabs/UI/UIRoot");
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot);
            _rootContainer.RegisterInstance(_uiRoot);
            
            var saveLoadSystem = new PlayerPrefsSaveLoadSystem();
            _rootContainer.RegisterInstance(saveLoadSystem);
            
            _rootContainer.RegisterFactory(_ => new GameData()).AsSingle();
            _rootContainer.RegisterFactory(_ => new InputActions()).AsSingle();
            
            var gameSettings = new GameSettings(saveLoadSystem.Load("settings", new GameSettings()));
            _rootContainer.RegisterInstance(gameSettings);
        }

        private void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.GAMEPLAY)
            {
                return;
            }

            if (sceneName == Scenes.TRIBE)
            {
                Coroutines.StartRoutine(LoadScene(Scenes.TRIBE));
            }

            if (sceneName == Scenes.MAIN_MENU)
            {
                Coroutines.StartRoutine(LoadScene(Scenes.MAIN_MENU));
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif
            Coroutines.StartRoutine(LoadScene(Scenes.MAIN_MENU));
        }

        public IEnumerator LoadScene(string sceneName)
        {
            _uiRoot.ShowLoadingScreen();
            _uiRoot.ClearSceneUI();

            yield return SceneManager.LoadSceneAsync(Scenes.BOOT);
            yield return SceneManager.LoadSceneAsync(sceneName, scene => _currentScene = scene);
            _currentScene.Initialize(_rootContainer);

            yield return new WaitForSeconds(1f);
            
            _currentScene.StartScene();
            
            _uiRoot.HideLoadingScreen();
        }
    }
}
