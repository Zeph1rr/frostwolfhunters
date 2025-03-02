using UnityEngine;
using System;

public class GameSettings
{

    public string CurrentResolution {get; private set;}
    public bool IsFullscreen {get; private set;}
    public float Volume {get; private set;}
    public string Language {get; private set;}

    public GameSettings()
    {
        CurrentResolution = "1920x1080";
        IsFullscreen = true;
        Volume = 1;
        Language = "English";
    }

    public GameSettings(string currentResolution, bool isFullscreen, float volume, string language)
    {
        CurrentResolution = currentResolution;
        IsFullscreen = isFullscreen;
        Volume = volume;
        Language = language;
    }

    public GameSettings(GameSettings settings) {
        CurrentResolution = settings.CurrentResolution;
        IsFullscreen = settings.IsFullscreen;
        Volume = settings.Volume;
        Language = settings.Language;
        Debug.Log($"{CurrentResolution}, {IsFullscreen}, {Volume}, {Language}");
    }

    public void SetVolume(float value)
    {
        if (value < 0 || value > 1) throw new ArgumentOutOfRangeException();
        Volume = value;
    }

    public void SetResolution(string resolution) {
        CurrentResolution = resolution;
    }

    public void SetFullscreen(bool fullscreen)
    {
        IsFullscreen = fullscreen;
    }

    public void SetLanguage(string language)
    {
        Language = language;
    }
}
