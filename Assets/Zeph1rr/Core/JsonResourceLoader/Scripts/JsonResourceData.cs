using System.Collections.Generic;

[System.Serializable]
public class JsonResourceData
{
    public List<JsonResourceList> lists;

    public Dictionary<string, Dictionary<string, string>> ToDictionary()
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var list in lists)
        {
            result[list.entry] = new Dictionary<string, string>();
            foreach (var entry in list.entries)
            {
                result[list.entry][entry.key] = entry.value;
            }
        }
        return result;
    }
}

[System.Serializable]
public class JsonResourceList
{
    public string entry;
    public List<JsonResourceEntry> entries;
}

[System.Serializable]
public class JsonResourceEntry
{
    public string key;
    public string value;
}