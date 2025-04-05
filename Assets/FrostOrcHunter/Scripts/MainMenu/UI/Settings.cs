using System.Collections.Generic;
using FrostOrcHunter.Scripts.Data;
using FrostOrcHunter.Scripts.GameRoot.Localization;
using FrostOrcHunter.Scripts.GameRoot.UI;
using UnityEngine;
using UnityEngine.UI;
using Zeph1rrGameBase.Scripts.Core.DI;
using Zeph1rrGameBase.Scripts.Core.SaveLoadSystem;

namespace FrostOrcHunter.Scripts.MainMenu.UI
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private UIDropdown _resolutionDropdown;
        [SerializeField] private UIDropdown _languageDropdown;
        [SerializeField] private Toggle _fullscreenToggle;
        [SerializeField] private Slider _volumeSlider;

        private List<string> _resolutions;
        private List<string> _languages;
        private GameSettings _settings;
        private PlayerPrefsSaveLoadSystem _saveLoadSystem;

        public void Initialize(DIContainer container)
        {
            _settings = container.Resolve<GameSettings>();
            _saveLoadSystem = container.Resolve<PlayerPrefsSaveLoadSystem>();
            InitializeResolutionDropdown();
            InitializeLanguageDropdown();
            InitializeFullscreenToggle();
            InitializeVolumeSlider();
        }

        public void SaveSettings()
        {
            _saveLoadSystem.Save("settings", _settings);
        }
        
        private void InitializeFullscreenToggle()
        {
            _fullscreenToggle.onValueChanged.RemoveAllListeners();
            _fullscreenToggle.SetIsOnWithoutNotify(_settings.IsFullscreen);
            _fullscreenToggle.onValueChanged.AddListener(_settings.SetFullscreen);
        }

        private void InitializeLanguageDropdown()
        {
            _languages = new List<string>();
            foreach (var language in LocalizationSystem.GetAllKeys())
            {
                _languages.Add(language);
            }
            _languageDropdown.Initialize(_languages, _settings.Language, SetLanguage);
        }

        private void InitializeResolutionDropdown()
        {
            _resolutions = new List<string>();
            var resolutions = Screen.resolutions;

            foreach (var t in resolutions)
            {
                var option = t.width + " x " + t.height;
                _resolutions.Add(option);
            }
            
            _resolutionDropdown.Initialize(_resolutions, _settings.CurrentResolution, SetResolution);
        }

        private void InitializeVolumeSlider()
        {
            _volumeSlider.onValueChanged.RemoveAllListeners();
            _volumeSlider.SetValueWithoutNotify(_settings.Volume);
            _volumeSlider.onValueChanged.AddListener(_settings.SetVolume);
        }
        
        private void SetResolution(int resolutionIndex)
        {
            _settings.SetResolution(_resolutions[resolutionIndex]);
        }

        private void SetLanguage(int languageIndex)
        {
            _settings.SetLanguage(_languages[languageIndex]);
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }
    }
}