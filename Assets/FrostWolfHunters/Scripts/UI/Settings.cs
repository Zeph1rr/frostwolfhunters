using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private TMP_Dropdown _languagesDropdown;
    private Resolution[] _resolutions;
    private List<string> _languages;

    private void Start()
    {
        LocalizationSystem.SetLanguage(_gameSettings.Language);
        SetFullscreen(_gameSettings.IsFullscreen);
        List<string> resolutionOptions = new List<string>();
        _resolutions = Screen.resolutions;
        SetResolution(_gameSettings.CurrentResolutionIndex);
        int currentResolutionIndex = 0;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;
            resolutionOptions.Add(option);
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
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

    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        _gameSettings.setFullscreen(isFullscreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        _gameSettings.SetResolution(resolutionIndex);
    }

    public void SetLanguage(int languageIndex)
    {
        string language = _languages[languageIndex];
        LocalizationSystem.SetLanguage(language);
        _gameSettings.SetLanguage(language);
        foreach (var localizedText in FindObjectsByType<LocalizedText>(FindObjectsSortMode.None))
        {
            localizedText.UpdateText();
        }
    }

    public void SaveSettings() 
    {
        throw new NotImplementedException();
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        throw new NotImplementedException();
    }
}
