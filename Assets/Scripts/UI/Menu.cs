using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameData _defaultGameData;
    [SerializeField] private GameData _gameData;

    [SerializeField] private PlayerStatsSO _playerStats;
    [SerializeField] private PlayerStatsSO _basePlayerStats;

    [SerializeField] private Button _loadButton;

    private void Awake()
    {
        _defaultGameData.PlayerStats.Initialize(_basePlayerStats);
        if (!SaveLoadSystem.GetSaveFiles().Any())
        {
            _loadButton.interactable = false;
        }

    }

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
        _playerStats.Initialize(_gameData.PlayerStats);
        StartGame();
    }

    private void StartGame() {
        SceneManager.LoadScene("Gameplay");
    }
}
