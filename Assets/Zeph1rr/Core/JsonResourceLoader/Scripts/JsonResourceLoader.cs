
using UnityEngine;
using System.Collections.Generic;

public static class JsonResourceLoader
{
    public static Dictionary<string, Dictionary<string, string>> LoadResources(string path)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(path);
        if (jsonFile == null)
        {
            Debug.LogError("Translations file not found!");
            return null;
        }

        JsonResourceData resourcesData = JsonUtility.FromJson<JsonResourceData>(jsonFile.text);
        return resourcesData.ToDictionary();
    }
}