using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

        using (FileStream fileStream = new FileStream(saveFilePath, FileMode.OpenOrCreate))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, gameData);
        }
        Debug.Log($"Game saved to {saveFilePath}");
    }

    public static List<string> GetSaveFiles()
    {
        if (!Directory.Exists(_saveDirectory))
        {
            Debug.LogWarning("Save directory not found!");
            return new List<string>();
        }

        string[] files = Directory.GetFiles(_saveDirectory, "*.save");
        List<string> saveFileNames = new List<string>();

        foreach(string filePath in files)
        {
            saveFileNames.Add(Path.GetFileName(filePath));
        }
        return saveFileNames;
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
}
