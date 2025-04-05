using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Tribe.RandomEvents
{
    public static class RandomEventSystem
    {
        private static string Language { get; set; }
        private static Dictionary<string, RandomEventList> _randomEvents;

        public static void SetLanguage(string language)
        {
            Language = language;
        }

        private static void LoadData()
        {
            var jsonFile = Resources.Load<TextAsset>($"RandomEvents/{Language}");
            if (!jsonFile)
            {
                Debug.LogError("RandomEvents file not found!");
                return;
            }

            var randomEventData = JsonUtility.FromJson<RandomEventData>(jsonFile.text);
            _randomEvents = randomEventData.ToDictionary();
        }

        public static T CreateEvent<T>() where T : RandomEvent
        {
            LoadData();
            if (_randomEvents == null || _randomEvents.Count == 0 || !_randomEvents.ContainsKey(typeof(T).Name))
            {
                Debug.LogError($"RandomEvent named {typeof(T).Name} not found!");
                return null;
            }
            var data = _randomEvents[typeof(T).Name];
            return (T)Activator.CreateInstance(typeof(T), data.Title, data.Description, data.OnFailureExpand, data.OnSuccessExpand);
        }
    }
}