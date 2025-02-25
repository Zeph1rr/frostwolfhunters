using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour, ISceeneRoot
{
    [Header("Stats")]
    [SerializeField] private GameData _defaultGameData;
    [SerializeField] private GameData _gameData;
    [SerializeField] private PlayerStatsSO _playerStats;
    [SerializeField] private PlayerStatsSO _basePlayerStats;

    [Header("UI")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _saveSelector;
    [SerializeField] private GameObject _playerName;
    [SerializeField] private GameObject _loadButtonPrefab;
    [SerializeField] private GameObject _settings;
    [SerializeField] private Button _loadButton;
    [SerializeField] private TMP_InputField _playerNameInputField;
    private PlayerInputActions _playerInput;

    public void StartScene()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.Enable();
        _playerInput.Global.Escape.performed += Escape_performed;
        _menu.SetActive(true);
        UpdateLocalizedText();
        _defaultGameData.PlayerStats.Initialize(_basePlayerStats);
        if (SaveLoadSystem.GetSaveFiles().Length == 0)
        {
            _loadButton.interactable = false;
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void NewGameButton()
    {
        _menu.SetActive(false);
        UpdateLocalizedText();
        _playerName.SetActive(true);
        _playerNameInputField.text = $"{LocalizationSystem.Translate("hunter")}{SaveLoadSystem.GetSaveFiles().Length + 1}";
        
    }

    public void Escape_performed(InputAction.CallbackContext context)
    {
        ReturnToMenu();
    }

    public void ReturnToMenu()
    {
        _playerName.SetActive(false);
        _saveSelector.SetActive(false);
        _settings.SetActive(false);
        _menu.SetActive(true);
        UpdateLocalizedText();
    }

    public void OpenSettings()
    {
        _menu.SetActive(false);
        _settings.SetActive(true);
        UpdateLocalizedText();
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
        UpdateLocalizedText();
    }

    private void OnDestroy()
    {
        _playerInput.Disable();
    }

    private void UpdateLocalizedText()
    {
        foreach (var localizedText in FindObjectsByType<LocalizedText>(FindObjectsSortMode.None))
        {
            localizedText.UpdateText();
        }
    }

    private void CreateLoadList()
    {
        string[] saveFiles = SaveLoadSystem.GetSaveFiles();
        saveFiles = saveFiles
            .OrderByDescending(file => File.GetLastWriteTime(file)) 
            .ToArray();
        GridLayoutGroup saveSelector = _saveSelector.GetComponentInChildren<GridLayoutGroup>();
        foreach (Transform child in saveSelector.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(string filePath in saveFiles)
        {
            GameObject newButton = Instantiate(_loadButtonPrefab, saveSelector.transform);
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
