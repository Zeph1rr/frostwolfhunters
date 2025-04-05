using System;
using FrostOrcHunter.Scripts.GameRoot.Localization;
using FrostOrcHunter.Scripts.Tribe.RandomEvents;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data
{
    [Serializable]
    public class GameSettings
    {
        [SerializeField] private string _currentResolution;
        [SerializeField] private bool _isFullScreen;
        [SerializeField] private float _volume;
        [SerializeField] private string _language;
        public string CurrentResolution => _currentResolution;
        public bool IsFullscreen => _isFullScreen;
        public float Volume => _volume;
        public string Language => _language;

        public GameSettings()
        {
            _currentResolution = "1920 x 1080";
            _isFullScreen = true;
            _volume = 1f;
            _language = "English";
        }

        public GameSettings(string currentResolution, bool isFullscreen, float volume, string language)
        {
            SetResolution(currentResolution);
            SetFullscreen(isFullscreen);
            SetVolume(volume);
            SetLanguage(language);
        }

        public GameSettings(GameSettings settings)
        {
            SetResolution(settings.CurrentResolution);
            SetFullscreen(settings.IsFullscreen);
            SetVolume(settings.Volume);
            SetLanguage(settings.Language);
            //Debug.Log($"{CurrentResolution}, {IsFullscreen}, {Volume}, {Language}");
        }

        public void SetVolume(float value)
        {
            if (value < 0 || value > 1) throw new ArgumentOutOfRangeException();
            _volume = value;
        }

        public void SetResolution(string resolution)
        {
            _currentResolution = resolution;
            string[] data = resolution.Split('x');
            Screen.SetResolution(int.Parse(data[0]), int.Parse(data[1]), Screen.fullScreen);
        }

        public void SetFullscreen(bool fullscreen)
        {
            _isFullScreen = fullscreen;
            Screen.fullScreen = fullscreen;
        }

        public void SetLanguage(string language)
        {
            _language = language;
            //Debug.Log(language);
            LocalizationSystem.SetLanguage(language);
            RandomEventSystem.SetLanguage(language);
        }
    }
}