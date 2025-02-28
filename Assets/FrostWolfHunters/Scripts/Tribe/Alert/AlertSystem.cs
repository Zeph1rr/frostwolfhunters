using System.Collections.Generic;
using UnityEngine;

public static class AlertSystem
{

    private static Dictionary<string, Dictionary<string, string>> _alerts;
    private static string _currentLanguage = "English";

    private static void LoadAlerts()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>($"Alerts/{_currentLanguage}");
        if (jsonFile == null)
        {
            Debug.LogError("Alerts file not found!");
            return;
        }

        AlertData alertData = JsonUtility.FromJson<AlertData>(jsonFile.text);
        _alerts = alertData.ToDictionary();
    }  

    public static void SetCurrentLanguage(string language)
    {
        _currentLanguage = language;
    }

    public static Dictionary<string, string> GetAlert(string key)
    {
        LoadAlerts();

        if (_alerts != null && _alerts.TryGetValue(key, out var alertDict))
        {
            return alertDict;
        }
        return null;
    }
}