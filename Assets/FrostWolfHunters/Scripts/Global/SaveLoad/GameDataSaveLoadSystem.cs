using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Zeph1rr.Core.SaveLoad;

public class GameDataSaveLoadSystem : SaveLoadSystem<GameData>
{
    public GameDataSaveLoadSystem(string saveDirectory) : base(saveDirectory)
    {
    }

    public override GameData Load(string saveFileName, GameData defaultData)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(SaveDirectory, saveFileName);
        if (!File.Exists(saveFilePath)) {
            Debug.LogWarning("Save file not found! Returning default game data");
            return defaultData;
        }

        using (FileStream fileStream = new(saveFilePath, FileMode.Open))
        {
            BinaryFormatter formatter = new();
            GameDataSerializable gameData = (GameDataSerializable)formatter.Deserialize(fileStream);
            return gameData.Deserialize();
        }
    }

    public override void Save(GameData data, string saveFileName)
    {
        CreateSaveDirectory();
        string saveFilePath = Path.Combine(SaveDirectory, $"{saveFileName}.save");

        using (FileStream fileStream = new(saveFilePath, FileMode.OpenOrCreate))
        {
            BinaryFormatter formatter = new();
            formatter.Serialize(fileStream, new GameDataSerializable(data, new PlayerStatsSerializable(data.PlayerStats)));
        }
        
        Debug.Log($"Saved to {saveFilePath}");
    }
}
