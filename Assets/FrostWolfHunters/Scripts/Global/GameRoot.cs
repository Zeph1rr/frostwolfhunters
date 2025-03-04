using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zeph1rr.Core.SaveLoad;

public class GameRoot : MonoBehaviour
{
    private GameSettings _gameSettings;
    public BinarySaveLoadSystem<GameData, GameDataSerializable> GameDataSaveLoadSystem {get; private set;}
    public JsonSaveLoadSystem<GameSettings, SettingsSerializable> SettingsSaveLoadSystem {get; private set;}
    public GameSettings GameSettings => _gameSettings;
    private GameData _gameData;

    public static GameRoot Instance {get; private set;}


    private void Awake()
    {
        GameDataSaveLoadSystem = new(Path.Combine(Application.persistentDataPath, "save"));
        GameDataSaveLoadSystem.CreateSaveDirectory();
        SettingsSaveLoadSystem = new(Application.persistentDataPath);
        GameSettings defaultSettings = new();
        _gameSettings = SettingsSaveLoadSystem.Load("settings", defaultSettings);
        if (_gameSettings == defaultSettings)
        {
            SettingsSaveLoadSystem.Save(new SettingsSerializable(defaultSettings), "settings");
        }
        LocalizationSystem.SetLanguage(_gameSettings.Language);
        AlertSystem.SetCurrentLanguage(_gameSettings.Language);
        Utils.SetResolution(_gameSettings.CurrentResolution);
        Utils.SetFullScreen(_gameSettings.IsFullscreen);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ISceneCompositeRoot sceneRoot = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<ISceneCompositeRoot>().ToArray()[0];
        sceneRoot.StartScene(_gameData);
    }

    public void ChangeScene(string name)
    {
        if (name == "Gameplay")
        {
            _gameData.ResetWaveNumber();
            _gameData.HuntResourceStorage.ResetResourceStorage(Enum.GetNames(typeof(ResourceType)));
        } 
        SceneManager.LoadScene(name);
    }

    public void SaveAndLeaveToMainMenu()
    {
        SaveGame();
        ChangeScene("Menu");
    }

    public void SaveGame()
    {
        GameDataSaveLoadSystem.Save(_gameData.ToGameDataSerializable(), _gameData.PlayerName);
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
