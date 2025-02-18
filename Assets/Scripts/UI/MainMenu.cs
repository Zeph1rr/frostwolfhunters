using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private GameData _defaultGameData;
    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerStatsSO _playerStats;
    [SerializeField] private PlayerStatsSO _basePlayerStats;

    [Header("UI prefabs")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _saveSelector;
    [SerializeField] private GameObject _playerName;
    [SerializeField] private GameObject _loadButtonPrefab;
    [SerializeField] private Button _loadButton;
    [SerializeField] private TMP_InputField _playerNameInputField;


    public void QuitGame() {
        Application.Quit();
    }

    public void NewGameButton()
    {
        _menu.SetActive(false);
        _playerName.SetActive(true);
        _playerNameInputField.text = $"Player{SaveLoadSystem.GetSaveFiles().Length + 1}";
    }

    public void ApplyPlayerName()
    {
        if (SaveLoadSystem.IsSaveExists(_playerNameInputField.text))
        {
            Debug.LogWarning($"Save with name {_playerNameInputField.text}.save already exists!");
        }
        _playerStats.Initialize(_basePlayerStats);
        _gameData.Initialize(_playerStats, _defaultGameData.MaxWaveNumber, _defaultGameData.CurrentWaveNumber, _playerNameInputField.text);
        StartGame();
    }

    public void LoadGame(string fileName) {
        _gameData.Initialize(SaveLoadSystem.LoadGame(fileName, _defaultGameData, _playerStats));
        _playerStats.Initialize(_gameData.PlayerStats);
        StartGame();
    }

    public void LoadButton() {
        CreateLoadList();
        _menu.SetActive(false);
        _saveSelector.SetActive(true);
    }

    private void Awake()
    {
        _defaultGameData.PlayerStats.Initialize(_basePlayerStats);
        if (SaveLoadSystem.GetSaveFiles().Length == 0)
        {
            _loadButton.interactable = false;
        }
    }

    private void CreateLoadList()
    {
        string[] saveFiles = SaveLoadSystem.GetSaveFiles();
        foreach(string filePath in saveFiles)
        {
            GameObject newButton = Instantiate(_loadButtonPrefab, _saveSelector.transform);
            newButton.name = Path.GetFileNameWithoutExtension(filePath);

            Button button = newButton.GetComponent<Button>();
            if (button != null) {
                button.onClick.AddListener(() => LoadGame(SaveLoadSystem.GetSaveFileName(filePath)));
            }

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null) {
                buttonText.text = $"{Path.GetFileNameWithoutExtension(filePath)} - {SaveLoadSystem.GetSaveFileLastWriteTime(filePath)}"; 
            }
        }
    }

    private void StartGame() {
        SceneManager.LoadScene("Gameplay");
    }
}
