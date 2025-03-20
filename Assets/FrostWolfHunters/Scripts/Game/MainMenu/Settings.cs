using System.Collections.Generic;
using BaCon;
using FrostWolfHunters.Scripts.Game.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zeph1rr.Core.Localization;
using Zeph1rr.Core.Utils;

namespace FrostWolfHunters.Scripts.Game.MainMenu
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _resolutionDropdown;
        [SerializeField] private TMP_Dropdown _languagesDropdown;
        [SerializeField] private Toggle _fullscreenToggle;
        private GameSettings _gameSettings;
        private Resolution[] _resolutions;
        private List<string> _languages;

        public void Initialize(DIContainer container)
        {
            _gameSettings = container.Resolve<GameSettings>();
            List<string> resolutionOptions = new List<string>();
            _resolutions = Screen.resolutions;
            int currentResolutionIndex = 0;
            string[] currentResolutionData = _gameSettings.CurrentResolution.Split('x');

            for (int i = 0; i < _resolutions.Length; i++)
            {
                string option = _resolutions[i].width + "x" + _resolutions[i].height;
                resolutionOptions.Add(option);
            
                if (_resolutions[i].width == int.Parse(currentResolutionData[0]) && _resolutions[i].height == int.Parse(currentResolutionData[1]))
                {
                    currentResolutionIndex = i;
                }
            }
            _resolutionDropdown.ClearOptions();
            _resolutionDropdown.AddOptions(resolutionOptions);
            _resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
            _resolutionDropdown.RefreshShownValue();
        
            _languagesDropdown.ClearOptions();
            _languages = LocalizationSystem.GetAllKeys();
            _languagesDropdown.AddOptions(_languages);
            _languagesDropdown.SetValueWithoutNotify(_languages.FindIndex(a => a == _gameSettings.Language));
            _languagesDropdown.RefreshShownValue();

            _fullscreenToggle.SetIsOnWithoutNotify(_gameSettings.IsFullscreen);

        }

        public void SetFullscreen(bool isFullscreen)
        {
            ScreenUtils.SetFullScreen(isFullscreen);
            _gameSettings.SetFullscreen(isFullscreen);
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = _resolutions[resolutionIndex];
            string resolutionString = resolution.width + "x" + resolution.height;
            ScreenUtils.SetResolution(resolutionString);
            _gameSettings.SetResolution(resolutionString);
        }

        public void SetVolume(float value)
        {
            Debug.Log(value);
            _gameSettings.SetVolume(value);
        }

        public void SetLanguage(int languageIndex)
        {
            string language = _languages[languageIndex];
            LocalizationSystem.SetLanguage(language);
            AlertSystem.SetCurrentLanguage(language);
            _gameSettings.SetLanguage(language);
            Debug.Log(language);
            foreach (var localizedText in FindObjectsByType<LocalizedText>(FindObjectsSortMode.None))
            {
                localizedText.UpdateText();
            }
        }

        public void SaveSettings() 
        {
            global::GameRoot.Instance.SettingsSaveLoadSystem.Save("settings", _gameSettings);
        }
    }
}
