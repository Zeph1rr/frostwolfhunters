using System.Collections;
using System.IO;
using BaCon;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.MainMenu.Root;
using FrostWolfHunters.Scripts.Game.Tribe.Root;
using R3;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeph1rr.Core.Localization;
using Zeph1rr.Core.SaveLoad;
using Zeph1rr.Core.Utils;

namespace FrostWolfHunters.Scripts.Game.GameRoot
{
    public class GameEntryPoint
    {
        public JsonSaveLoadSystem SettingsSaveLoadSystem { get; private set; }
        public Base64SaveLoadSystem GameDataSaveLoadSystem { get; private set; }

        private static GameEntryPoint _instance;
        private readonly GameSettings _gameSettings;
        private readonly UIRoot _uiRoot;
        private readonly DIContainer _rootContainer = new();
        private DIContainer _cachedSceneContainer;
        private GameData _gameData;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutostartGame()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _instance ??= new GameEntryPoint();
            _instance.RunGame();


        }

        private GameEntryPoint()
        {
            var prefabUIRoot = Resources.Load<UIRoot>("UIRoot");
            _uiRoot = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRoot);
            _rootContainer.RegisterInstance(_uiRoot);
            
            GameDataSaveLoadSystem = new Base64SaveLoadSystem(Path.Combine(Application.persistentDataPath, "save"));
            GameDataSaveLoadSystem.CreateSaveDirectory();
            _rootContainer.RegisterInstance(GameDataSaveLoadSystem);

            SettingsSaveLoadSystem = new JsonSaveLoadSystem(Application.persistentDataPath);
            _rootContainer.RegisterInstance(SettingsSaveLoadSystem);

            GameSettings defaultSettings = new();
            _gameSettings = SettingsSaveLoadSystem.Load("settings", defaultSettings);
            if (_gameSettings == defaultSettings)
            {
                SettingsSaveLoadSystem.Save("settings", defaultSettings);
            }
            _rootContainer.RegisterInstance(_gameSettings);
            _rootContainer.RegisterFactory(_ => new GameData()).AsSingle();

            LocalizationSystem.SetLanguage(_gameSettings.Language);
            AlertSystem.SetCurrentLanguage(_gameSettings.Language);
            ScreenUtils.SetResolution(_gameSettings.CurrentResolution);
            ScreenUtils.SetFullScreen(_gameSettings.IsFullscreen);
        }

        private void RunGame()
        {
#if UNITY_EDITOR
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Scenes.HUNT)
            {
                return;
            }

            if (sceneName == Scenes.TRIBE)
            {
                Coroutines.StartRoutine(LoadAndStartTribe());
            }

            if (sceneName == Scenes.MAIN_MENU)
            {
                Coroutines.StartRoutine(LoadAndStartMainMenu());
                return;
            }

            if (sceneName != Scenes.BOOT)
            {
                return;
            }
#endif
            Coroutines.StartRoutine(LoadAndStartMainMenu());
        }

        private IEnumerator LoadAndStartMainMenu(MainMenuEnterParams enterParams = null)
        {
            _uiRoot.ShowLoadingScreen();
            _cachedSceneContainer?.Dispose();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.MAIN_MENU);
            
            yield return new WaitForSeconds(1f);
            
            
           
             var sceneEntryPoint = Object.FindFirstObjectByType<MainMenuEntryPoint>();
             var mainMenuContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
             sceneEntryPoint.Run(mainMenuContainer, enterParams).Subscribe(mainMenuExitParams =>
             {
                 var targetSceneName = mainMenuExitParams.TargetSceneEnterParams.SceneName;

                 if (targetSceneName == Scenes.TRIBE)
                 {
                     Coroutines.StartRoutine(LoadAndStartTribe(mainMenuExitParams.TargetSceneEnterParams.As<TribeEnterParams>()));
                 }
             });
             
            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadAndStartTribe(TribeEnterParams enterParams = null)
        {
            _uiRoot.ShowLoadingScreen();
            _cachedSceneContainer?.Dispose();
            
            yield return LoadScene(Scenes.BOOT);
            yield return LoadScene(Scenes.TRIBE);
            
            yield return new WaitForSeconds(1f);
            
            var sceneEntryPoint = Object.FindFirstObjectByType<TribeEntryPoint>();
            var tribeContainer = _cachedSceneContainer = new DIContainer(_rootContainer);
            sceneEntryPoint.Run(tribeContainer, enterParams).Subscribe(tribeExitParams =>
            {
                var targetSceneName = tribeExitParams.TargetSceneEnterParams.SceneName;

                if (targetSceneName == Scenes.MAIN_MENU)
                {
                    Coroutines.StartRoutine(LoadAndStartMainMenu(tribeExitParams.TargetSceneEnterParams.As<MainMenuEnterParams>()));
                }
            });
            
            _uiRoot.HideLoadingScreen();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}