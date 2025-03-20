using System.IO;
using System.Linq;
using BaCon;
using FrostWolfHunters.Scripts.Game.Data;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zeph1rr.Core.Localization;
using Zeph1rr.Core.SaveLoad;

namespace FrostWolfHunters.Scripts.Game.MainMenu.Root.View
{
    public class UIMainMenuRootBinder : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _saveSelector;
        [SerializeField] private GameObject _playerName;
        [SerializeField] private GameObject _loadButtonPrefab;
        [SerializeField] private GameObject _settings;
        [SerializeField] private Button _loadButton;
        [SerializeField] private TMP_InputField _playerNameInputField;

        private PlayerInputActions _playerInput;
        private GameData _gameData;
        private Base64SaveLoadSystem _saveLoadSystem;
        private Subject<Unit> _exitSceneSignalSubj;
        
        public void StartScene(DIContainer container)
        {
            _gameData = container.Resolve<GameData>();
            _playerInput = new PlayerInputActions();
            _playerInput.Enable();
            _playerInput.Global.Escape.performed += Escape_performed;
            _menu.SetActive(true);
            UpdateLocalizedText();
            _saveLoadSystem = container.Resolve<Base64SaveLoadSystem>();
            if (_saveLoadSystem.GetSaveFiles().Length == 0)
            {
                _loadButton.interactable = false;
            }
            var settings = _settings.GetComponent<Settings>();
            settings.Initialize(container);
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
                Debug.LogWarning($"Save with name {_playerNameInputField.text}.{_saveLoadSystem.FileExtension} already exists!");
            }
            _gameData = new(_playerNameInputField.text);
            StartGame();
        }

        public void LoadGame(string fileName) {
            var gameData = new GameData(_saveLoadSystem.Load(fileName, new GameData()));
            _gameData.UpdateGameData(gameData);
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
            foreach(var filePath in saveFiles)
            {
                var newButton = Instantiate(_loadButtonPrefab, saveSelector.transform);
                newButton.name = Path.GetFileNameWithoutExtension(filePath);

                var button = newButton.GetComponent<Button>();
                if (button != null) {
                    button.onClick.AddListener(() => LoadGame(_saveLoadSystem.GetSaveFileName(filePath)));
                }

                var buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null) {
                    buttonText.text = $"{Path.GetFileNameWithoutExtension(filePath)} - {_saveLoadSystem.GetSaveFileLastWriteTime(filePath)}"; 
                }
            }
        }
        
        private void StartGame() {
            _exitSceneSignalSubj?.OnNext(Unit.Default);
        }

        public void Bind(Subject<Unit> exitSceneSignalSubj)
        {
            _exitSceneSignalSubj = exitSceneSignalSubj;
        }
    }
}