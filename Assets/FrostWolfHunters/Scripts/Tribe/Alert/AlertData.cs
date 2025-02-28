using System.Collections.Generic;

[System.Serializable]
public class AlertData
{
    public List<AlertNameEntry> alerts;

    public Dictionary<string, Dictionary<string, string>> ToDictionary()
    {
        var result = new Dictionary<string, Dictionary<string, string>>();
        foreach (var alert in alerts)
        {
            result[alert.alert] = new Dictionary<string, string>();
            foreach (var entry in alert.entries)
            {
                result[alert.alert][entry.key] = entry.value;
            }
        }
        return result;
    }
}

[System.Serializable]
public class AlertNameEntry
{
    public string alert;
    public List<AlertEntry> entries;
}

[System.Serializable]
public class AlertEntry
{
    public string key;
    public string value;
}