using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeph1rr.Core.SaveLoad;
using Zeph1rr.Core.Utils;
using FrostWolfHunters.Scripts.Game.Data;
using FrostWolfHunters.Scripts.Game.Data.Enums;
using FrostWolfHunters.Scripts.Game.GameRoot;
using Zeph1rr.Core.Localization;

public class GameRoot : MonoBehaviour
{
    private GameSettings _gameSettings;
    public Base64SaveLoadSystem GameDataSaveLoadSystem { get; private set; }
    public JsonSaveLoadSystem SettingsSaveLoadSystem { get; private set; }
    public GameSettings GameSettings => _gameSettings;
    private GameData _gameData;

    public static GameRoot Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        GameDataSaveLoadSystem = new Base64SaveLoadSystem(Path.Combine(Application.persistentDataPath, "save"));
        GameDataSaveLoadSystem.CreateSaveDirectory();

        SettingsSaveLoadSystem = new JsonSaveLoadSystem(Application.persistentDataPath);

        GameSettings defaultSettings = new();
        _gameSettings = SettingsSaveLoadSystem.Load("settings", defaultSettings);
        if (_gameSettings == defaultSettings)
        {
            SettingsSaveLoadSystem.Save("settings", defaultSettings);
        }

        LocalizationSystem.SetLanguage(_gameSettings.Language);
        AlertSystem.SetCurrentLanguage(_gameSettings.Language);
        ScreenUtils.SetResolution(_gameSettings.CurrentResolution);
        ScreenUtils.SetFullScreen(_gameSettings.IsFullscreen);

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != Scenes.MAIN_MENU && scene.name != Scenes.BOOT)
        {
            ISceneCompositeRoot sceneRoot = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                .OfType<ISceneCompositeRoot>().ToArray()[0];
            sceneRoot.StartScene(_gameData);
        }
    }

    public void ChangeScene(string sceneName)
    {
        if (sceneName == "Gameplay")
        {
            _gameData.ResetWaveNumber();
            _gameData.HuntResourceStorage.ResetResourceStorage(Enum.GetNames(typeof(ResourceType)));
        }
        SceneManager.LoadScene(sceneName);
    }

    public void SaveAndLeaveToMainMenu()
    {
        SaveGame();
        ChangeScene("Menu");
    }

    public void SaveGame()
    {
        GameDataSaveLoadSystem.Save(_gameData.PlayerName, _gameData);
    }

    public void SetGameData(GameData gameData)
    {
        _gameData = gameData;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}