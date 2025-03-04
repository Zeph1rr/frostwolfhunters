using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using UnityEngine.InputSystem;
using Zeph1rr.Core.SaveLoad;

public class MainMenu : MonoBehaviour, ISceneCompositeRoot
{
    private PlayerStats _playerStats;

    [Header("UI")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _saveSelector;
    [SerializeField] private GameObject _playerName;
    [SerializeField] private GameObject _loadButtonPrefab;
    [SerializeField] private GameObject _settings;
    [SerializeField] private Button _loadButton;
    [SerializeField] private TMP_InputField _playerNameInputField;
    private PlayerInputActions _playerInput;
    public PlayerStats playerStats => _playerStats;
    private GameData _gameData;
    private BinarySaveLoadSystem<GameData, GameDataSerializable> _saveLoadSystem;

    public void StartScene(GameData gameData)
    {
        _gameData = gameData;
        _playerInput = new PlayerInputActions();
        _playerInput.Enable();
        _playerInput.Global.Escape.performed += Escape_performed;
        _menu.SetActive(true);
        UpdateLocalizedText();
        _saveLoadSystem = GameRoot.Instance.GameDataSaveLoadSystem;
        if (_saveLoadSystem.GetSaveFiles().Length == 0)
        {
            _loadButton.interactable = false;
        }
        Settings settings = _settings.GetComponent<Settings>();
        settings.Initialize();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void NewGameButton()
    {
        _menu.SetActive(false);
        _playerName.SetActive(true);
        UpdateLocalizedText();
        _playerNameInputField.text = $"{LocalizationSystem.Translate("hunter")}{_saveLoadSystem.GetSaveFiles().Length + 1}";
        
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
        if (_saveLoadSystem.IsSaveExists(_playerNameInputField.text))
        {
            Debug.LogWarning($"Save with name {_playerNameInputField.text}.save already exists!");
        }
        _gameData = new(_playerNameInputField.text);
        StartGame();
    }

    public void LoadGame(string fileName) {
        _gameData = new GameData(_saveLoadSystem.Load(fileName, new GameData()));
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
        string[] saveFiles = _saveLoadSystem.GetSaveFiles();
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
                button.onClick.AddListener(() => LoadGame(_saveLoadSystem.GetSaveFileName(filePath)));
            }

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null) {
                buttonText.text = $"{Path.GetFileNameWithoutExtension(filePath)} - {_saveLoadSystem.GetSaveFileLastWriteTime(filePath)}"; 
            }
        }
    }

    private void StartGame() {
        GameRoot.Instance.SetGameData(_gameData);
        SceneManager.LoadScene("Tribe");
    }
}
