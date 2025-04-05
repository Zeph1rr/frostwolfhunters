using System.Collections.Generic;

namespace FrostOrcHunter.Scripts.GameRoot.Localization
{
    [System.Serializable]
    public class LocalizationData
    {
        public List<LanguageEntry> languages;

        public Dictionary<string, Dictionary<string, string>> ToDictionary()
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            foreach (var language in languages)
            {
                result[language.language] = new Dictionary<string, string>();
                foreach (var entry in language.entries)
                {
                    result[language.language][entry.key] = entry.value;
                }
            }
            return result;
        }
    }

    [System.Serializable]
    public class LanguageEntry
    {
        public string language;
        public List<TranslationEntry> entries;
    }

    [System.Serializable]
    public class TranslationEntry
    {
        public string key;
        public string value;
    }
}