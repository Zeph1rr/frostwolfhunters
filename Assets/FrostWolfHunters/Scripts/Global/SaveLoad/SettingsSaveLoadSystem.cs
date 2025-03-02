using System.IO;
using UnityEngine;
using Zeph1rr.Core.SaveLoad;

public class SettingsSaveLoadSystem : SaveLoadSystem<GameSettings>
{
    public SettingsSaveLoadSystem(string saveDirectory) : base(saveDirectory)
    {
    }

    public override GameSettings Load(string saveFileName, GameSettings defaultData)
    {
        string saveFilePath = Path.Combine(SaveDirectory, "settings.json");
        if (!File.Exists(saveFilePath)) {
            Debug.LogWarning("Settings file not found! Returning default settings");
            Save(defaultData, saveFileName);
            return defaultData;
        }
        string json = File.ReadAllText(saveFilePath);
        SettingsSerializable settings = JsonUtility.FromJson<SettingsSerializable>(json);
        return settings.Deserialize();
    }

    public override void Save(GameSettings data, string saveFileName)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(SaveDirectory, "settings.json");

        string json = JsonUtility.ToJson(new SettingsSerializable(data), true);
        File.WriteAllText(saveFilePath, json);
        
        Debug.Log($"Saved to {saveFilePath}");
    }
}
