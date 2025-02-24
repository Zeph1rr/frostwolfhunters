using System;
using UnityEngine;

[Serializable]
public class SettingsSerializable
{
    [SerializeField] private string _currentResolution;
    [SerializeField] private bool _isFullscreen;
    [SerializeField] private float _volume;
    [SerializeField] private string _language;

    public SettingsSerializable(string currentResolution, bool isFullscreen, float volume, string language) {
        _currentResolution = currentResolution;
        _isFullscreen = isFullscreen;
        _volume = volume;
        _language = language;
    }

    public SettingsSerializable(GameSettings settings) {
        _currentResolution = settings.CurrentResolution;
        _isFullscreen = settings.IsFullscreen;
        _volume = settings.Volume;
        _language = settings.Language;
    }

    public GameSettings Desirialize() 
    {
        GameSettings settings = ScriptableObject.CreateInstance<GameSettings>();
        settings.Initialize(_currentResolution, _isFullscreen, _volume, _language);
        return settings;
    }
}