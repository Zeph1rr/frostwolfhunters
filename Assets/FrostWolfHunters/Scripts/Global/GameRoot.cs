using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    private GameSettings _gameSettings;
    public GameDataSaveLoadSystem GameDataSaveLoadSystem {get; private set;}
    public SettingsSaveLoadSystem SettingsSaveLoadSystem {get; private set;}
    public GameSettings GameSettings => _gameSettings;
    [SerializeField] private GameData _gameData;

    public static GameRoot Instance {get; private set;}


    private void Awake()
    {
        GameDataSaveLoadSystem = new(Path.Combine(Application.persistentDataPath, "save"));
        GameDataSaveLoadSystem.CreateSaveDirectory();
        SettingsSaveLoadSystem = new(Application.persistentDataPath);
        GameSettings defaultSettings = new();
        _gameSettings = SettingsSaveLoadSystem.Load("", defaultSettings);
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
        if (name == "Gameplay") _gameData.ResetWaveNumber();
        SceneManager.LoadScene(name);
    }

    public void SaveAndLeaveToMainMenu()
    {
        GameDataSaveLoadSystem.Save(_gameData, _gameData.PlayerName);
        ChangeScene("Menu");
    }

    private void OnDestroy()
    {  
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
