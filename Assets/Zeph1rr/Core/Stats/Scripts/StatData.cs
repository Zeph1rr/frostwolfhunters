using System.Collections.Generic;


namespace Zeph1rr.Core.Stats
{
    [System.Serializable]
    public class StatEntryList
    {
        public List<StatEntry> stats;
    }

    [System.Serializable]
    public class StatEntry
    {
        public string stat;
        public List<StatData> entries;
    }

    [System.Serializable]
    public class StatData
    {
        public int level;
        public float value;
    }
}

