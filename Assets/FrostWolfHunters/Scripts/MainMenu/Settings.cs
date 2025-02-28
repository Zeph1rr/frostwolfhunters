using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private TMP_Dropdown _languagesDropdown;
    [SerializeField] private Toggle _fullscreenToggle;
    private Resolution[] _resolutions;
    private List<string> _languages;

    private void Start()
    {
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
                Debug.Log(i);
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
        Utils.SetFullScreen(isFullscreen);
        _gameSettings.setFullscreen(isFullscreen);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        string resolution_string = resolution.width + "x" + resolution.height;
        Utils.SetResolution(resolution_string);
        _gameSettings.SetResolution(resolution_string);
    }

    public void SetLanguage(int languageIndex)
    {
        string language = _languages[languageIndex];
        LocalizationSystem.SetLanguage(language);
        AlertSystem.SetCurrentLanguage(language);
        _gameSettings.SetLanguage(language);
        foreach (var localizedText in FindObjectsByType<LocalizedText>(FindObjectsSortMode.None))
        {
            localizedText.UpdateText();
        }
    }

    public void SaveSettings() 
    {
        SaveLoadSystem.SaveSettings(new SettingsSerializable(_gameSettings));
    }
}
