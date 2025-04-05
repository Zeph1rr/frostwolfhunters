using System;
using System.IO;
using UnityEngine;

namespace FrostOrcHunter.Scripts.Data.Stats
{
    public class StatsLoader
    {
        private static StatEntryList _stats;

        public static float GetStatValue(string name, int level)
        {
            LoadStats();
            var stat = FindStatByName(name);
            return FindValueByLevel(stat, level);
        }

        private static StatEntry FindStatByName(string name)
        {
            return _stats.stats.Find(statEntry => statEntry.stat == name);
        }

        private static float FindValueByLevel(StatEntry statEntry, int level)
        {
            var statData = statEntry.entries.Find(stat => stat.level == level);
            if (statData == null) throw new ArgumentOutOfRangeException("This stat don't have this level");
            return statData.value;
        }

        private static void LoadStats()
        {
            var jsonFile = UnityEngine.Resources.Load<TextAsset>("Stats/stats");
            if (jsonFile == null) throw new FileNotFoundException("Can't find recource /Stats/stats.json");
            
            _stats = JsonUtility.FromJson<StatEntryList>(jsonFile.text);
        }
    }
}