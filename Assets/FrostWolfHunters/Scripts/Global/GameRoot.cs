using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameSettings _defaultSettings;
    [SerializeField] private GameObject _sceneRoot;
    [SerializeField] private GameData _gameData;

    private void Awake()
    {
        ISceeneRoot sceeneRoot = _sceneRoot.GetComponent<ISceeneRoot>();
        _gameSettings.Initialize(SaveLoadSystem.LoadSettings(_defaultSettings));
        LocalizationSystem.SetLanguage(_gameSettings.Language);
        AlertSystem.SetCurrentLanguage(_gameSettings.Language);
        Utils.SetResolution(_gameSettings.CurrentResolution);
        Utils.SetFullScreen(_gameSettings.IsFullscreen);
        sceeneRoot.StartScene(_gameData);
    }

    public void ChangeScene(string name)
    {
        if (name == "Gameplay") _gameData.ResetWaveNumber();
        SceneManager.LoadScene(name);
    }

    public void SaveAndLeaveToMainMenu()
    {
        SaveGame();
        ChangeScene("Menu");
    }

    private void SaveGame() {
        PlayerStatsSerializable playerStats = new(_gameData.PlayerStats);
        GameDataSerializable gameData = new(_gameData, playerStats);
        SaveLoadSystem.SaveGame(gameData, _gameData.PlayerName);
    }
}
