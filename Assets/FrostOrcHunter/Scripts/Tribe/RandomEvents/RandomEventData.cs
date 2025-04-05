using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Tribe.RandomEvents
{
    [Serializable]
    public class RandomEventData
    {
        public List<RandomEventEntry> Entries;

        public Dictionary<string, RandomEventList> ToDictionary()
        {
            var dictionary = new Dictionary<string, RandomEventList>();
            foreach (var entry in Entries)
            {
                dictionary.Add(entry.Name, entry.RandomEvent);
            }
            return dictionary;
        }
    }
    
    [Serializable]
    public class RandomEventEntry
    {
        public string Name;
        public RandomEventList RandomEvent;
    }
    
    [Serializable]
    public class RandomEventList
    {
        public string Title;
        public string Description;
        public string OnSuccessExpand;
        public string OnFailureExpand;
    }
}