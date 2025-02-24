using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Game/Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField] private string _currentResolution;
    [SerializeField] private bool _isFullscreen;
    [SerializeField][Range(0,1)] private float _volume;
    [SerializeField] private string _language;

    public string CurrentResolution => _currentResolution;
    public bool IsFullscreen => _isFullscreen;
    public float Volume => _volume;
    public string Language => _language;

    public void Initialize(string currentResolution, bool isFullscreen, float volume, string language)
    {
        _currentResolution = currentResolution;
        _isFullscreen = isFullscreen;
        _volume = volume;
        _language = language;
    }

    public void Initialize(GameSettings settings) {
        _currentResolution = settings.CurrentResolution;
        _isFullscreen = settings.IsFullscreen;
        _volume = settings.Volume;
        _language = settings.Language;
    }

    public void SetVolume(float value)
    {
        if (value < 0 || value > 1) throw new ArgumentOutOfRangeException();
        _volume = value;
    }

    public void SetResolution(string resolution) {
        _currentResolution = resolution;
    }

    public void setFullscreen(bool fullscreen)
    {
        _isFullscreen = fullscreen;
    }

    public void SetLanguage(string language)
    {
        _language = language;
    }
}
