using System.IO;
using UnityEngine;

namespace Zeph1rr.Core.Stats
{
    public class StatsLoader
    {
        private static StatEntryList _stats;

        public static float GetStatValue(string name, int level)
        {
            LoadStats();
            StatEntry stat = FindStatByName(name);
            return FindValueByLevel(stat, level);
        }

        private static StatEntry FindStatByName(string name)
        {
            return _stats.stats.Find(statEntry => statEntry.stat == name);
        }

        private static float FindValueByLevel(StatEntry statEntry, int level)
        {
            StatData statData = statEntry.entries.Find(stat => stat.level == level);
            return statData.value;
        }

        private static void LoadStats()
        {
            TextAsset jsonFile = UnityEngine.Resources.Load<TextAsset>("Stats/stats");
            if (jsonFile == null) throw new FileNotFoundException("Can't find recource /Stats/stats.json");
            
            _stats = JsonUtility.FromJson<StatEntryList>(jsonFile.text);
        }
    }
}

