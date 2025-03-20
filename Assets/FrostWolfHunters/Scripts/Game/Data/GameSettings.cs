using System;
using UnityEngine;

namespace FrostWolfHunters.Scripts.Game.Data
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
            _currentResolution = "1920x1080";
            _isFullScreen = true;
            _volume = 1;
            _language = "English";
        }

        public GameSettings(string currentResolution, bool isFullscreen, float volume, string language)
        {
            _currentResolution = currentResolution;
            _isFullScreen = isFullscreen;
            _volume = volume;
            _language = language;
        }

        public GameSettings(GameSettings settings)
        {
            _currentResolution = settings.CurrentResolution;
            _isFullScreen = settings.IsFullscreen;
            _volume = settings.Volume;
            _language = settings.Language;
            Debug.Log($"{CurrentResolution}, {IsFullscreen}, {Volume}, {Language}");
        }

        public void SetVolume(float value)
        {
            if (value < 0 || value > 1) throw new ArgumentOutOfRangeException();
            _volume = value;
        }

        public void SetResolution(string resolution)
        {
            _currentResolution = resolution;
        }

        public void SetFullscreen(bool fullscreen)
        {
            _isFullScreen = fullscreen;
        }

        public void SetLanguage(string language)
        {
            _language = language;
        }
    }
}