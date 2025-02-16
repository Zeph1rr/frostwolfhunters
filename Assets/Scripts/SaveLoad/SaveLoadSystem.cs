using System;
using System.IO;
using UnityEngine;

public class SaveLoadSystem
{
    private static string _saveDirectory = Path.Combine(Application.persistentDataPath, "save"); 
    private static void CreateSaveDirectory() {
        if (!Directory.Exists(_saveDirectory)) {
            Directory.CreateDirectory(_saveDirectory);
        }
    }

    public static void SaveGame(GameDataSerializable gameData, string saveFileName)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(_saveDirectory, saveFileName);
        string json = JsonUtility.ToJson(gameData, true);
        Debug.Log(json);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved to {saveFilePath}");
    }

    public static GameData LoadGame(string saveFileName, GameData defaultGameData)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(Application.persistentDataPath, "save", saveFileName);
        if (!File.Exists(saveFilePath)) {
            Debug.LogWarning("Save file not found! Returning default game data");
            return defaultGameData;
        }

        string json = File.ReadAllText(saveFilePath);
        GameDataSerializable gameDataSerializable = JsonUtility.FromJson<GameDataSerializable>(json);
        GameData gameData = gameDataSerializable.Deserialize();
        return gameData;
    }
}
