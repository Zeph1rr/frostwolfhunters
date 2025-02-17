using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameData _defaultGameData;
    [SerializeField] GameData _gameData;

    [SerializeField] PlayerStatsSO _playerStats;
    [SerializeField] PlayerStatsSO _basePlayerStats;

    public void QuitGame() {
        Application.Quit();
    }

    public void NewGame() {
        _gameData.Initialize(_defaultGameData);
        _playerStats.Initialize(_basePlayerStats);
        StartGame();
    }

    public void LoadGame() {
        _gameData.Initialize(SaveLoadSystem.LoadGame("test.save", _defaultGameData, _playerStats));
        Debug.Log(_gameData.PlayerStats);
        _playerStats.Initialize(_gameData.PlayerStats);
        StartGame();
    }

    private void StartGame() {
        SceneManager.LoadScene("Gameplay");
    }
}
