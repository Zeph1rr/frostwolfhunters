using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadSystem
{
    private static string _saveDirectory = Path.Combine(Application.persistentDataPath, "save"); 
    private static string _settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.json");
    private static void CreateSaveDirectory() {
        if (!Directory.Exists(_saveDirectory)) {
            Directory.CreateDirectory(_saveDirectory);
        }
    }

    public static bool IsSaveExists(string playerName)
    {
        return File.Exists(Path.Combine(_saveDirectory, $"{playerName}.save"));
    }

    public static void SaveGame(GameDataSerializable gameData, string saveFileName)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(_saveDirectory, saveFileName);

        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, gameData);
        }
        Debug.Log($"Game saved to {saveFilePath}");
    }

    public static string[] GetSaveFiles()
    {
        if (!Directory.Exists(_saveDirectory))
        {
            Debug.LogWarning("Save directory not found!");
            return new string[0];
        }

        return Directory.GetFiles(_saveDirectory, "*.save");
    }

    public static string GetSaveFileLastWriteTime(string filePath)
    {
        if (File.Exists(filePath))
        {
            return File.GetLastWriteTime(filePath).ToString("yyyy-MM-dd HH:mm:ss");
        }
        else
        {
            Debug.LogWarning($"File not found: {filePath}");
            return "File not found";
        }
    }

    public static string GetSaveFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }

    public static GameData LoadGame(string saveFileName, GameData defaultGameData, PlayerStatsSO stats)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(Application.persistentDataPath, "save", saveFileName);
        Debug.Log(saveFilePath);
        if (!File.Exists(saveFilePath)) {
            Debug.LogWarning("Save file not found! Returning default game data");
            return defaultGameData;
        }

        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            GameDataSerializable gameDataSerializable = (GameDataSerializable)formatter.Deserialize(fileStream);
            return gameDataSerializable.Deserialize(stats);
        }
    }

    public static void SaveSettings(SettingsSerializable settings)
    {

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(_settingsFilePath, json);
        Debug.Log($"Settings saved to {_settingsFilePath}");
    }

    public static GameSettings LoadSettings(GameSettings defaultGameSettings)
    {
        if (!File.Exists(_settingsFilePath)) {
            Debug.LogWarning("Settings file not found! Returning default settings");
            SaveSettings(new SettingsSerializable(defaultGameSettings));
            return defaultGameSettings;
        }
        string json = File.ReadAllText(_settingsFilePath);
        SettingsSerializable settings = JsonUtility.FromJson<SettingsSerializable>(json);
        GameSettings gameSettings = settings.Desirialize();
        return gameSettings;
    }
}
