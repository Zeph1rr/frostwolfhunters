using System.Collections.Generic;
using UnityEngine;

namespace FrostOrcHunter.Scripts.GameRoot.Localization
{
    public static class LocalizationSystem
    {

        private static Dictionary<string, Dictionary<string, string>> _translations;
        private static string _currentLanguage = "English";

        private static void LoadTranslations()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Localization/translations");
            if (!jsonFile)
            {
                Debug.LogError("Translations file not found!");
                return;
            }

            LocalizationData localizationData = JsonUtility.FromJson<LocalizationData>(jsonFile.text);
            _translations = localizationData.ToDictionary();
        }    

        public static void SetLanguage(string language)
        {
            LoadTranslations();

            if (_translations != null && _translations.ContainsKey(language))
            {
                _currentLanguage = language;
                return;
            }
            Debug.LogWarning($"Language {language} not found in translations file!");
        }

        public static string Translate(string key)
        {
            LoadTranslations();

            if (_translations != null && _translations.TryGetValue(_currentLanguage, out var langDict))
            {
                if (langDict.TryGetValue(key, out var value))
                {
                    return value;
                }
            }
            return key;
        }

        public static List<string> GetAllKeys()
        {
            LoadTranslations();

            return new List<string>(_translations.Keys);
        }

    }
}